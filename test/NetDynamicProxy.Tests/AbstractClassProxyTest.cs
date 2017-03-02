using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NetDynamicProxy.Tests
{
    public class AbstractClassProxyTest
    {
		[Fact]
		public void ShouldConstructProxyForAbstractClass()
		{
			AbstractClass proxy = Proxifier.For<AbstractClass>((instance, method, args) => null).Build();
			Assert.IsAssignableFrom(typeof(object), proxy);
		}

		[Fact]
		public void ShouldWireCallback()
		{
			bool proxyCalled = false;
			AbstractClass proxy = Proxifier.For<AbstractClass>((instance, method, args) => {
				proxyCalled = true;
				return null;
			}).Build();
			proxy.Method();
			Assert.True(proxyCalled, "The proxy callback was not invoked");
		}

		[Fact]
		public void ShouldWireCallbackOnMethodWithArgs()
		{
			bool proxyCalled = false;
			Object[] proxyArgs = null;
			AbstractClass proxy = Proxifier.For<AbstractClass>((instance, method, args) => {
				proxyCalled = true;
				proxyArgs = args;
				return null;
			}).Build();
			proxy.MethodWithArgs(100, "Proxy method test");
			Assert.True(proxyCalled, "The proxy callback was not invoked");
			Assert.Equal(new object[] { 100, "Proxy method test" }, proxyArgs);
		}

		[Fact]
		public void ShouldUseProxyCallbackValueOnMethodWithReferenceTypeReturnValue()
		{
			const string ReturnValue = "Test string";
			AbstractClass proxy = Proxifier.For<AbstractClass>((instance, method, args) => {
				return ReturnValue;
			}).Build();
			String result = proxy.MethodWithReferenceTypeReturnValue();
			Assert.Equal(ReturnValue, result);
		}

		[Fact]
		public void ShouldUseProxyCallbackValueOnMethodWithValueTypeReturnValue()
		{
			const int ReturnValue = 1234;
			AbstractClass proxy = Proxifier.For<AbstractClass>((instance, method, args) => {
				return 1234;
			}).Build();
			int result = proxy.MethodWithValueTypeReturnValue();
			Assert.Equal(ReturnValue, result);
		}
	}

	public abstract class AbstractClass
	{
		public abstract void Method();

		public abstract void MethodWithArgs(int arg1, String arg2);

		public abstract String MethodWithReferenceTypeReturnValue();

		public abstract int MethodWithValueTypeReturnValue();
	}


}
