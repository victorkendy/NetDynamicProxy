using System;
using System.Reflection;

namespace NetDynamicProxy
{
	public class InvocationContext
	{
		public InvocationContext(Object proxyInstance, MethodInfo method, Object[] args)
		{
			this.ProxyInstance = proxyInstance;
			this.Method = method;
			this.Arguments = args;
		}

		public object[] Arguments { get; private set; }
		public MethodInfo Method { get; private set; }
		public object ProxyInstance { get; private set; }

		public Object InvokeRealMethod()
		{
			return this.Method.Invoke(ProxyInstance, Arguments);
		}
	}
}