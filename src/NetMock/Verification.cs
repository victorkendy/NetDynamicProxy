using NetDynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMock
{
    public class Verification
    {
		public static T Verify<T>(T mock) where T : class
		{
			return DoVerify(mock, v => v.TimesCalled == 1);
		}

		private static T DoVerify<T>(T mock, Func<MockVerification, bool> verificationMode) where T : class
		{
			return Proxifier.For<T>(new VerificationProxyAction((MockProxyAction)Proxifier.GetProxyAction(mock), verificationMode)).Build();
		}
	}

	public class MockVerification
	{
		internal MockVerification(int invocationsMatching, MockProxyAction mockAction)
		{
			this.TimesCalled = invocationsMatching;
			this.MockAction = mockAction;
		}

		internal int TimesCalled { get; private set; }
		internal MockProxyAction MockAction { get; private set; }
	}
}
