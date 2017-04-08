using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace NetDynamicProxy
{
	internal class ProxyFactory
	{
		internal static readonly String AssemblyName = "NetDynamicProxy__ProxyFactory__Internal";
		private AssemblyBuilder assemblyBuilder;
		private ModuleBuilder moduleBuilder;

		internal void Init()
		{
			this.assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new System.Reflection.AssemblyName(AssemblyName), AssemblyBuilderAccess.RunAndCollect);
			this.moduleBuilder = assemblyBuilder.DefineDynamicModule("Proxies");
		}

		internal Object Create(Type baseClass, IList<Type> implementedInterfaces, IProxyAction action)
		{
			String proxyTypeName = baseClass.FullName + "__Proxy";
			TypeAttributes proxyTypeAttr = TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Public;
			var typeBuilder = moduleBuilder.DefineType(proxyTypeName, proxyTypeAttr, baseClass, implementedInterfaces.ToArray());

			var callbackType = typeof(IProxyAction);
			var callbackField = typeBuilder.DefineField("proxy__callback", callbackType, FieldAttributes.Private);
			defineConstructor(typeBuilder, callbackField);
			overrideVirtualMethods(typeBuilder, baseClass, callbackField);
			foreach(var @interface in implementedInterfaces)
			{
				overrideVirtualMethods(typeBuilder, @interface, callbackField);
			}

			var proxyType = typeBuilder.CreateTypeInfo();
			return proxyType.GetConstructor(new Type[] { callbackType }).Invoke(new Object[] { action });
		}

		private void defineConstructor(TypeBuilder typeBuilder, FieldBuilder callbackField)
		{
			var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { callbackField.FieldType });
			var il = constructor.GetILGenerator();
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Call, typeof(Object).GetConstructor(Type.EmptyTypes));
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldarg_1);
			il.Emit(OpCodes.Stfld, callbackField);
			il.Emit(OpCodes.Ret);
		}

		private void overrideVirtualMethods(TypeBuilder typeBuilder, Type type, FieldBuilder callbackField)
		{
			var methods = type.GetRuntimeMethods();
			foreach (var method in methods)
			{
				if (method.IsVirtual || method.IsAbstract)
				{
					MethodAttributes methodAttributes = MethodAttributes.Public | MethodAttributes.Virtual;
					var argumentTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
					var methodBuilder = typeBuilder.DefineMethod(method.Name, methodAttributes, method.ReturnType, argumentTypes);
					var il = methodBuilder.GetILGenerator();
					var args = il.DeclareLocal(typeof(Object[]));
					il.Emit(OpCodes.Ldarg_0);
					il.Emit(OpCodes.Ldfld, callbackField);
					il.Emit(OpCodes.Ldarg_0);
					il.Emit(OpCodes.Ldtoken, method);
					il.Emit(OpCodes.Call, typeof(MethodBase).GetMethod("GetMethodFromHandle", new Type[] { typeof(RuntimeMethodHandle) }));
					il.Emit(OpCodes.Ldc_I4, method.GetParameters().Length);
					il.Emit(OpCodes.Newarr, typeof(Object));
					il.Emit(OpCodes.Stloc, args);
					for (int i = 1; i <= method.GetParameters().Length; i++)
					{
						var param = method.GetParameters()[i - 1];
						il.Emit(OpCodes.Ldloc, args);
						il.Emit(OpCodes.Ldc_I4, i - 1);
						il.Emit(OpCodes.Ldarg_S, (byte)i);
						if (param.ParameterType.GetTypeInfo().IsValueType)
						{
							il.Emit(OpCodes.Box, param.ParameterType);
						}
						il.Emit(OpCodes.Stelem_Ref);
					}

					il.Emit(OpCodes.Ldloc, args);
					il.Emit(OpCodes.Newobj, typeof(InvocationContext).GetConstructor(new Type[] { typeof(Object), typeof(MethodInfo), typeof(Object[]) }));
					il.Emit(OpCodes.Callvirt, callbackField.FieldType.GetMethod("OnMethodInvoked", new Type[] { typeof(InvocationContext) }));
					if(method.ReturnType.Equals(typeof(void)))
					{
						il.Emit(OpCodes.Pop);
					}
					else if(method.ReturnType.GetTypeInfo().IsValueType)
					{
						il.Emit(OpCodes.Unbox_Any, method.ReturnType);
					}
					il.Emit(OpCodes.Ret);
					if(!type.GetTypeInfo().IsInterface)
					{
						typeBuilder.DefineMethodOverride(methodBuilder, method);
					}
				}
			}
		}
	}
}