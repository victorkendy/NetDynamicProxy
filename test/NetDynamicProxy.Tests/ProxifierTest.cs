using NetDynamicProxy;
using System;
using Xunit;

namespace Tests
{
	public class ProxifierTest
	{
		[Fact]
		public void ShouldConstructProxyForObject()
		{
			Assert.IsAssignableFrom(typeof(object), Proxifier.For<Object>((instance, method, args) => null).Build());
		}

		[Fact]
		public void ShouldConstructProxyForConcreteClass()
		{
			ConcreteClass proxy = Proxifier.For<ConcreteClass>((instance, method, args) => null).Build();
			Assert.IsAssignableFrom(typeof(object), proxy);
		}

		[Fact]
		public void ShouldWireCallbackOnConcreteClassCallback()
		{
			bool proxyCalled = false;
			ConcreteClass proxy = Proxifier.For<ConcreteClass>((instance, method, args) => {
				proxyCalled = true;
				return null;
			}).Build();
			proxy.Method();
			Assert.True(proxyCalled, "The proxy callback was not invoked");
		}

		[Fact]
		public void ShouldWireCallbackOnMethodsWithArgs()
		{
			bool proxyCalled = false;
			Object[] proxyArgs = null;
			ConcreteClass proxy = Proxifier.For<ConcreteClass>((instance, method, args) => {
				proxyCalled = true;
				proxyArgs = args;
				return null;
			}).Build();
			proxy.MethodWithArgs(100, "Proxy method test");
			Assert.True(proxyCalled, "The proxy callback was not invoked");
			Assert.Equal(new object[] { 100, "Proxy method test" }, proxyArgs);
		}
	}

	public class ConcreteClass
	{
		public virtual void Method()
		{
			throw new NotImplementedException("Test method");
		}

		public virtual void MethodWithArgs(int arg1, String arg2)
		{
			throw new NotImplementedException("Test method");
		}
	}
}
