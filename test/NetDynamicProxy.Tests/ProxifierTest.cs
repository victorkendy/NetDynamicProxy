using NetDynamicProxy;
using System;
using Xunit;

namespace NetDynamicProxy.Tests
{
	public class ProxifierTest
	{
		[Fact]
		public void ShouldThrowArgumentExceptionIfBaseTypeIsAInterface()
		{
			Assert.Throws<ArgumentException>(() =>
			{
				Proxifier.For<BaseInterface>(new FuncProxyAction((instance, method, args) => null));
			});
		}
		[Fact]
		public void ShouldThrowArgumentNullExceptionIfProxyCallbackIsNull()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				Proxifier.For<Object>(null);
			});
		}
		[Fact]
		public void ShouldThrowArgumentExceptionWhenPassingAClassAsInterface()
		{
			Assert.Throws<ArgumentException>(() =>
			{
				Proxifier.For<Object>(new FuncProxyAction((instance, method, args) => null))
				.WithInterfaces(typeof(ClassAsInterface));
			});
		}
		[Fact]
		public void ShouldThrowArgumentNullExceptionWhenCallingWithInterfacesWithoutAnyInterfaces()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				Proxifier.For<Object>(new FuncProxyAction((instance, method, args) => null))
				.WithInterfaces();
			});
		}

		[Fact]
		public void ShouldThrowArgumentNullExceptionWhenCallingWithInterfacesWithNull()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				Proxifier.For<Object>(new FuncProxyAction((instance, method, args) => null))
				.WithInterfaces(typeof(BaseInterface), null);
			});
		}

		[Fact]
		public void ShouldReturnProxyAction()
		{
			var proxyAction = new FuncProxyAction((instance, method, args) => null);
			BaseInterface proxy = Proxifier.WithoutBaseClass(proxyAction).WithInterfaces(typeof(BaseInterface)).Build<BaseInterface>();

			Assert.Same(proxyAction, Proxifier.GetProxyAction(proxy));
		}
	}

	public class ClassAsInterface
	{

	}

	public interface BaseInterface
	{

	}
}
