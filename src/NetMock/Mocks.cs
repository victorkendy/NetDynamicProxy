using NetDynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMock
{
    public class Mocks
    {
		public static OnGoingStubbing<T> When<T>(T ignored)
		{
			MockContext.LastInvokedMock.PrepareForStubbing();
			return new OnGoingStubbing<T>();
		}

		public static T Mock<T>() where T:class
		{
			return Proxifier.For<T>(new MockProxyAction()).Build();
		}

		public static MockStubbing DoReturn<T>(T returnValue)
		{
			return new MockStubbing();
		}
    }

	public class OnGoingStubbing<T>
	{
		private readonly MockProxyAction MockProxy;

		internal OnGoingStubbing()
		{
			this.MockProxy = MockContext.LastInvokedMock;
		}

		public OnGoingStubbing<T> ThenThrow(Exception exception)
		{
			this.MockProxy.RecordAnswer(invocation => { throw exception; });
			return this;
		}

		public OnGoingStubbing<T> ThenThrow<E>() where E : Exception
		{
			this.MockProxy.RecordAnswer(invocation => { throw (Exception) Activator.CreateInstance(typeof(E)); });
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
