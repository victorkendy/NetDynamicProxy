using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NetMock.Tests
{
    public class VerificationTest
    {
		[Fact]
		public void ShouldNotThrowExceptionWhenVerifiedMethodIsCalled()
		{
			Object mock = Mocks.Mock<Object>();
			mock.ToString();

			Verification.Verify(mock).ToString();
		}

		[Fact]
		public void ShouldThrowExceptionWhenVerifiedMethodIsCalled()
		{
			Object mock = Mocks.Mock<Object>();

			Assert.Throws<MockVerificationFailedException>(() => Verification.Verify(mock).ToString());
		}

		[Fact]
		public void ShouldThrowExceptionWhenVerifiedMethodIsCalledMultipleTimes()
		{
			Object mock = Mocks.Mock<Object>();
			mock.ToString();
			mock.ToString();

			Assert.Throws<MockVerificationFailedException>(() => Verification.Verify(mock).ToString());
		}
	}
}
