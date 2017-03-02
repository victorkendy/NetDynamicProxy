using System;
using System.Collections.Generic;
using System.Reflection;

namespace NetDynamicProxy
{
	public class Proxifier
	{
		public static ProxifierWithBaseClass<T> For<T>(Func<Object, MethodInfo, Object[], Object> callback) where T : class
		{
			return new ProxifierWithBaseClass<T>(callback);
		}

		public static ProxifierWithBaseClass<Object> WithoutBaseClass(Func<Object, MethodInfo, Object[], Object> callback)
		{
			return new ProxifierWithBaseClass<Object>(callback);
		}
	}

	public class ProxifierWithBaseClass<T> where T : class
	{
		private IList<Type> implementedInterfaces = new List<Type>();
		private Func<Object, MethodInfo, Object[], Object> callback;

		public ProxifierWithBaseClass(Func<Object, MethodInfo, Object[], Object> callback)
		{
			this.callback = callback;
		}

		public ProxifierWithBaseClass<T> WithInterfaces(params Type[] interfaces)
		{
			foreach (Type i in interfaces)
			{
				implementedInterfaces.Add(i);
			}
			return this;
		}

		public T Build()
		{
			ProxyFactory factory = new ProxyFactory();
			factory.Init();
			return (T) factory.Create(typeof(T), implementedInterfaces, callback);
		}

		public TResult Build<TResult>()
		{
			ProxyFactory factory = new ProxyFactory();
			factory.Init();
			return (TResult) factory.Create(typeof(T), implementedInterfaces, callback);
		}
	}
}
