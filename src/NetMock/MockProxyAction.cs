using System;
using NetDynamicProxy;
using System.Collections.Generic;

namespace NetMock
{
	internal class MockProxyAction : IProxyAction
	{
		private MockContext context;
		private bool hasAnswerDefined = false;
		private LinkedList<Func<InvocationContext, Object>> answers = new LinkedList<Func<InvocationContext, object>>();

		internal MockProxyAction(MockContext context)
		{
			this.context = context;
			this.answers.AddLast(invocation => {
				return 0;
			});
		}

		public object OnMethodInvoked(InvocationContext invocation)
		{
			context.LastInvokedMock = this;
			var answer = answers.First;
			if(answers.Count > 1)
			{
				answers.RemoveFirst();
			}
			return answer.Value(invocation);
		}

		internal void RecordAnswer(Func<InvocationContext, Object> answer)
		{
			if(!hasAnswerDefined)
			{
				answers.RemoveFirst();
				hasAnswerDefined = true;
			}
			answers.AddLast(answer);
		}
	}
}