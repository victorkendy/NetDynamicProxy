using NetDynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMock
{
    public class Mocks
    {
		private static readonly MockContext Context = new MockContext();

		public static OnGoingStubbing<T> When<T>(T ignored)
		{
			return new OnGoingStubbing<T>(Context);
		}

		public static T Mock<T>() where T:class
		{
			return Proxifier.For<T>(new MockProxyAction(Context)).Build();
		}

		public static MockStubbing DoReturn<T>(T returnValue)
		{
			return new MockStubbing();
		}
    }

	public class OnGoingStubbing<T>
	{
		private readonly MockProxyAction MockProxy;

		internal OnGoingStubbing(MockContext context)
		{
			this.MockProxy = context.LastInvokedMock;
		}

		public OnGoingStubbing<T> ThenThrow(Exception exception)
		{
			this.MockProxy.RecordAnswer(invocation => { throw exception; });
			return this;
		}

		public OnGoingStubbing<T> ThenReturn(T value)
		{
			this.MockProxy.RecordAnswer(invocation => (Object) value);
			return this;
		}

		public OnGoingStubbing<T> ThenAnswer(Func<InvocationContext, T> answer)
		{
			this.MockProxy.RecordAnswer(invocation => answer(invocation));
			return this;
		}
	}

	public class MockStubbing
	{
		public T When<T>(T mockInstance)
		{
			return mockInstance;
		}
	}
}
