using System;
using System.Collections.Generic;
using System.Reflection;

namespace NetDynamicProxy
{
	public class Proxifier
	{
		public static ProxifierWithBaseClass<T> For<T>(IProxyAction action) where T : class
		{
			if(typeof(T).GetTypeInfo().IsInterface)
			{
				throw new ArgumentException("Proxy base type cannot be an interface");
			}
			if(action == null)
			{
				throw new ArgumentNullException("Proxy action is undefined");
			}
			return new ProxifierWithBaseClass<T>(action);
		}

		public static ProxifierWithBaseClass<Object> WithoutBaseClass(IProxyAction action)
		{
			return new ProxifierWithBaseClass<Object>(action);
		}
	}

	public class ProxifierWithBaseClass<T> where T : class
	{
		private IList<Type> implementedInterfaces = new List<Type>();
		private IProxyAction action;

		public ProxifierWithBaseClass(IProxyAction action)
		{
			this.action = action;
		}

		public ProxifierWithBaseClass<T> WithInterfaces(params Type[] interfaces)
		{
			if(interfaces == null || interfaces.Length == 0)
			{
				throw new ArgumentNullException("Interfaces cannot be empty");
			}
			foreach (Type i in interfaces)
			{
				if (i == null)
				{
					throw new ArgumentNullException("Interface to be implemented cannot be null");
				}
				if(!i.GetTypeInfo().IsInterface)
				{
					throw new ArgumentException("The method WithInterfaces can only be called with interfaces as arguments");
				}
				implementedInterfaces.Add(i);
			}
			return this;
		}

		public T Build()
		{
			ProxyFactory factory = new ProxyFactory();
			factory.Init();
			return (T) factory.Create(typeof(T), implementedInterfaces, action);
		}

		public TResult Build<TResult>()
		{
			ProxyFactory factory = new ProxyFactory();
			factory.Init();
			return (TResult) factory.Create(typeof(T), implementedInterfaces, action);
		}
	}
}
