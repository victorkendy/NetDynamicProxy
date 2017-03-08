using System;
using System.Reflection;

namespace NetDynamicProxy.Tests
{
	public class FuncProxyAction : IProxyAction
	{
		private readonly Func<object, MethodInfo, object[], Object> callback;

		public FuncProxyAction(Func<Object, MethodInfo, Object[], Object> callback)
		{
			this.callback = callback;
		}
		public object OnMethodInvoked(InvocationContext context)
		{
			return callback(context.ProxyInstance, context.Method, context.Arguments);
		}
	}
}
