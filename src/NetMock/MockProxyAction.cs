using System;
using NetDynamicProxy;
using System.Collections.Generic;
using System.Reflection;

namespace NetMock
{
	internal class MockProxyAction : IProxyAction
	{
		private static readonly Func<InvocationContext, Object> DefaultAnswer = (invocation) => {
			var returnType = invocation.Method.ReturnType;
			if (returnType.GetTypeInfo().IsValueType)
			{
				return Activator.CreateInstance(returnType);
			}
			return null;
		};

		private MockContext context;
		private bool stubbingFinalized = true;
		private LinkedList<Func<InvocationContext, Object>> answers = new LinkedList<Func<InvocationContext, object>>();

		internal MockProxyAction(MockContext context)
		{
			this.context = context;
		}

		public object OnMethodInvoked(InvocationContext invocation)
		{
			if(!stubbingFinalized)
			{
				throw new InvalidMockUsageException();
			}
			context.LastInvokedMock = this;
			Func<InvocationContext, Object> answer = DefaultAnswer;
			if(answers.Count > 0)
			{
				var firstAnswer = answers.First;
				answer = firstAnswer.Value;
				if(firstAnswer.Next != null)
				{
					answers.RemoveFirst();
				}
			}
			return answer(invocation);
		}

		internal void RecordAnswer(Func<InvocationContext, Object> answer)
		{
			answers.AddLast(answer);
			stubbingFinalized = true;
		}

		internal void PrepareForStubbing()
		{
			stubbingFinalized = false;
			answers.Clear();
		}
	}
}