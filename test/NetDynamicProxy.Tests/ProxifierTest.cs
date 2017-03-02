using NetDynamicProxy;
using System;
using Xunit;

namespace Tests
{
    public class ProxifierTest
    {
        [Fact]
        public void Test1() 
        {
            Assert.IsAssignableFrom(typeof(object), Proxifier.For<Object>().Build());
        }
    }
}
