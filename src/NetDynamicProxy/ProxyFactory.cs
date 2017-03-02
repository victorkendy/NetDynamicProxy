using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace NetDynamicProxy
{
	internal class ProxyFactory
	{
		private static readonly String AssemblyName = "NetDynamicProxy__ProxyFactory__Internal";
		private AssemblyBuilder assemblyBuilder;
		private ModuleBuilder moduleBuilder;

		internal void Init()
		{
			this.assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new System.Reflection.AssemblyName(AssemblyName), AssemblyBuilderAccess.RunAndCollect);
			this.moduleBuilder = assemblyBuilder.DefineDynamicModule("Proxies");
		}

		internal T Create<T>(IList<Type> implementedInterfaces, Func<Object, MethodInfo, Object[], Object> callback)
		{
			Type originalType = typeof(T);
			String proxyTypeName = originalType.FullName + "__Proxy";
			TypeAttributes proxyTypeAttr = TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Public;
			var typeBuilder = moduleBuilder.DefineType(proxyTypeName, proxyTypeAttr, originalType, implementedInterfaces.ToArray());

			overrideVirtualMethods(typeBuilder, originalType);

			var proxyType = typeBuilder.CreateTypeInfo();
			return (T)proxyType.GetConstructor(new Type[0]).Invoke(new object[0]);
		}

		private void overrideVirtualMethods(TypeBuilder typeBuilder, Type type)
		{
			var methods = type.GetRuntimeMethods();
			foreach (var method in methods)
			{
				if ((method.IsVirtual || method.IsAbstract) && !method.DeclaringType.Equals(typeof(object)))
				{
					MethodAttributes methodAttributes = MethodAttributes.Public | MethodAttributes.Virtual;
					var argumentTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
					var methodBuilder = typeBuilder.DefineMethod(method.Name, methodAttributes, method.ReturnType, argumentTypes);
					var il = methodBuilder.GetILGenerator();
					il.Emit(OpCodes.Ret);
					typeBuilder.DefineMethodOverride(methodBuilder, method);
				}
			}
		}
	}
}