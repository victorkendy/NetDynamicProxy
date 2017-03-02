using System;
using System.Collections.Generic;

namespace NetDynamicProxy
{
    public class Proxifier
    {
        public static ProxifierWithBaseClass<T> For<T>() where T:class
        {
            return new ProxifierWithBaseClass<T>();
        }
    }

    public class ProxifierWithBaseClass<T> where T:class
    {
        private IList<Type> implementedInterfaces = new List<Type>();

        public ProxifierWithBaseClass<T> WithInterfaces(params Type[] interfaces)
        {
            foreach(Type i in interfaces)
            {
                implementedInterfaces.Add(i);
            }
            return this;
        }

        public T Build()
        {
            ProxyFactory factory = new ProxyFactory();
            factory.Init();
            return factory.Create<T>(implementedInterfaces);
        }
    }
}
