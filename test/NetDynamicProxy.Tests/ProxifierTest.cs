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
				Proxifier.For<BaseInterface>((instance, method, args) => null);
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
				Proxifier.For<Object>((instance, method, args) => null)
				.WithInterfaces(typeof(ClassAsInterface));
			});
		}
		[Fact]
		public void ShouldThrowArgumentNullExceptionWhenCallingWithInterfacesWithoutAnyInterfaces()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				Proxifier.For<Object>((instance, method, args) => null)
				.WithInterfaces();
			});
		}

		[Fact]
		public void ShouldThrowArgumentNullExceptionWhenCallingWithInterfacesWithNull()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				Proxifier.For<Object>((instance, method, args) => null)
				.WithInterfaces(typeof(BaseInterface), null);
			});
		}
	}

	public class ClassAsInterface
	{

	}

	public interface BaseInterface
	{

	}
}
