using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NetDynamicProxy.Tests
{
    public class PureInterfaceProxyTest
    {
		[Fact]
		public void ShouldProxifyInterface()
		{
			Object proxy = Proxifier.WithoutBaseClass((instance, method, args) => null).WithInterfaces(typeof(TestInterface)).Build();
			Assert.IsAssignableFrom(typeof(TestInterface), proxy);
		}

		[Fact]
		public void ShouldWireCallback()
		{
			bool proxyCalled = false;
			TestInterface proxy = Proxifier.WithoutBaseClass((instance, method, args) => {
				proxyCalled = true;
				return null;
			})
			.WithInterfaces(typeof(TestInterface))
			.Build<TestInterface>();
			proxy.Method();
			Assert.True(proxyCalled, "The proxy callback was not invoked");
		}

		[Fact]
		public void ShouldWireCallbackOnMethodWithArgs()
		{
			bool proxyCalled = false;
			Object[] proxyArgs = null;
			TestInterface proxy = Proxifier.WithoutBaseClass((instance, method, args) => {
				proxyCalled = true;
				proxyArgs = args;
				return null;
			})
			.WithInterfaces(typeof(TestInterface))
			.Build<TestInterface>();
			proxy.MethodWithArgs(100, "Proxy method test");
			Assert.True(proxyCalled, "The proxy callback was not invoked");
			Assert.Equal(new object[] { 100, "Proxy method test" }, proxyArgs);
		}

		[Fact]
		public void ShouldUseProxyCallbackValueOnMethodWithReferenceTypeReturnValue()
		{
			const string ReturnValue = "Test string";
			TestInterface proxy = Proxifier.WithoutBaseClass((instance, method, args) => {
				return ReturnValue;
			})
			.WithInterfaces(typeof(TestInterface))
			.Build<TestInterface>();
			String result = proxy.MethodWithReferenceTypeReturnValue();
			Assert.Equal(ReturnValue, result);
		}

		[Fact]
		public void ShouldUseProxyCallbackValueOnMethodWithValueTypeReturnValue()
		{
			const int ReturnValue = 1234;
			TestInterface proxy = Proxifier.WithoutBaseClass((instance, method, args) => {
				return 1234;
			})
			.WithInterfaces(typeof(TestInterface))
			.Build<TestInterface>();
			int result = proxy.MethodWithValueTypeReturnValue();
			Assert.Equal(ReturnValue, result);
		}

		[Fact]
		public void ShouldAllowProxyMultipleInterfaces()
		{
			Object proxy = Proxifier.WithoutBaseClass((instance, method, args) => null)
				.WithInterfaces(typeof(TestInterface), typeof(TestInterface2))
				.Build();
			Assert.IsAssignableFrom(typeof(TestInterface), proxy);
			Assert.IsAssignableFrom(typeof(TestInterface2), proxy);
		}
	}

	public interface TestInterface
	{
		void Method();

		void MethodWithArgs(int arg1, String arg2);

		String MethodWithReferenceTypeReturnValue();

		int MethodWithValueTypeReturnValue();
	}

	public interface TestInterface2
	{
		void Method1();
	}
}
