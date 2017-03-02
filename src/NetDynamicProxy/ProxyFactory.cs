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

        internal T Create<T>(IList<Type> implementedInterfaces)
        {
            String proxyTypeName = typeof(T).FullName + "__Proxy";
            TypeAttributes proxyTypeAttr = TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Public;
            var typeBuilder = moduleBuilder.DefineType(proxyTypeName, proxyTypeAttr, typeof(T), implementedInterfaces.ToArray());

            var proxyType = typeBuilder.CreateTypeInfo();
            return (T)proxyType.GetConstructor(new Type[0]).Invoke(new object[0]);
        }
    }
}