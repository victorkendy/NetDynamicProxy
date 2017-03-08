using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NetDynamicProxy.Tests
{
    public class ConcreteClassProxyTest
    {
		[Fact]
		public void ShouldConstructProxyForConcreteClass()
		{
			ConcreteClass proxy = Proxifier.For<ConcreteClass>(new FuncProxyAction((instance, method, args) => null)).Build();
			Assert.IsAssignableFrom(typeof(object), proxy);
		}

		[Fact]
		public void ShouldWireCallback()
		{
			bool proxyCalled = false;
			ConcreteClass proxy = Proxifier.For<ConcreteClass>(new FuncProxyAction((instance, method, args) => {
				proxyCalled = true;
				return null;
			})).Build();
			proxy.Method();
			Assert.True(proxyCalled, "The proxy callback was not invoked");
		}

		[Fact]
		public void ShouldWireCallbackOnMethodWithArgs()
		{
			bool proxyCalled = false;
			Object[] proxyArgs = null;
			ConcreteClass proxy = Proxifier.For<ConcreteClass>(new FuncProxyAction((instance, method, args) => {
				proxyCalled = true;
				proxyArgs = args;
				return null;
			})).Build();
			proxy.MethodWithArgs(100, "Proxy method test");
			Assert.True(proxyCalled, "The proxy callback was not invoked");
			Assert.Equal(new object[] { 100, "Proxy method test" }, proxyArgs);
		}

		[Fact]
		public void ShouldUseProxyCallbackValueOnMethodWithReferenceTypeReturnValue()
		{
			const string ReturnValue = "Test string";
			ConcreteClass proxy = Proxifier.For<ConcreteClass>(new FuncProxyAction((instance, method, args) => {
				return ReturnValue;
			})).Build();
			String result = proxy.MethodWithReferenceTypeReturnValue();
			Assert.Equal(ReturnValue, result);
		}

		[Fact]
		public void ShouldUseProxyCallbackValueOnMethodWithValueTypeReturnValue()
		{
			const int ReturnValue = 1234;
			ConcreteClass proxy = Proxifier.For<ConcreteClass>(new FuncProxyAction((instance, method, args) => {
				return 1234;
			})).Build();
			int result = proxy.MethodWithValueTypeReturnValue();
			Assert.Equal(ReturnValue, result);
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

		public virtual String MethodWithReferenceTypeReturnValue()
		{
			throw new NotImplementedException("Test method");
		}

		public virtual int MethodWithValueTypeReturnValue()
		{
			throw new NotImplementedException("Test method");
		}
	}
}
