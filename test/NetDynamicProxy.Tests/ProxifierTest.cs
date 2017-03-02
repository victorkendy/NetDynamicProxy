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
			Assert.True(proxyCalled, "The proxy callback was not invoked");
		}
	}

	public class ConcreteClass
	{
		public virtual void Method()
		{
			throw new NotImplementedException("Test method");
		}
	}
}
