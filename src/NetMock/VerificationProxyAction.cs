using System;
using System.Linq;
using NetDynamicProxy;

namespace NetMock
{
	internal class VerificationProxyAction : IProxyAction
	{
		private MockProxyAction mockAction;
		private Func<MockVerification, bool> verificationMode;

		public VerificationProxyAction(MockProxyAction mockAction, Func<MockVerification, bool> verificationMode)
		{
			this.mockAction = mockAction;
			this.verificationMode = verificationMode;
		}

		public object OnMethodInvoked(InvocationContext context)
		{
			int invocationsMatching = mockAction.Invocations.Where(invocation => invocationMatching(invocation, context)).Count();
			if(!verificationMode(new MockVerification(invocationsMatching, mockAction)))
			{
				throw new MockVerificationFailedException();
			}

			return MockProxyAction.DefaultAnswer(context);
		}

		private bool invocationMatching(InvocationContext invocation, InvocationContext context)
		{
			if (invocation.Method != context.Method || invocation.Arguments.Length != context.Arguments.Length) return false;
			for(int i = 0; i < invocation.Arguments.Length; i++)
			{
				if (!Object.Equals(invocation.Arguments[i], context.Arguments[i])) return false;
			}
			return true;
		}
	}
}