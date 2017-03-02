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
	}
}
