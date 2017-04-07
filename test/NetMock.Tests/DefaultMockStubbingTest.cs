using NetMock;
using System;
using System.Numerics;
using Xunit;

namespace NetMock.Tests
{
    public class DefaultMockStubbingTest
    {
        [Fact]
        public void ShouldMockMethodReturnValue() 
        {
			TestClass mock = Mocks.Mock<TestClass>();
			Mocks.When(mock.Method()).ThenReturn(1);
			
			Assert.Equal(1, mock.Method());
		}

		[Fact]
		public void ShouldMockMultipleReturnValues()
		{
			TestClass mock = Mocks.Mock<TestClass>();
			Mocks.When(mock.Method()).ThenReturn(1).ThenReturn(2);

			Assert.Equal(1, mock.Method());
			Assert.Equal(2, mock.Method());
		}

		[Fact]
		public void ShouldMockMethodThrowingException()
		{
			TestClass mock = Mocks.Mock<TestClass>();
			var exception = new Exception();
			Mocks.When(mock.Method()).ThenThrow(exception);

			try
			{
				Assert.True(false);
			} catch (Exception e)
			{
				Assert.Same(exception, e);
			}
		}

		[Fact]
		public void ShouldUseGivenAnswer()
		{
			TestClass mock = Mocks.Mock<TestClass>();
			Mocks.When(mock.Method()).ThenAnswer(invocation => 20);

			Assert.Equal(20, mock.Method());
		}

		[Fact]
		public void ShouldStubMultipleMockInstances()
		{
			TestClass mock1 = Mocks.Mock<TestClass>();
			Mocks.When(mock1.Method()).ThenReturn(20);
			TestClass mock2 = Mocks.Mock<TestClass>();
			Mocks.When(mock2.Method()).ThenReturn(30);

			Assert.Equal(20, mock1.Method());
			Assert.Equal(30, mock2.Method());
		}
	}

	public class TestClass
	{
		public virtual int Method()
		{
			throw new NotImplementedException();
		}
	}
}
