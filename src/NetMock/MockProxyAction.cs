using System;
using NetDynamicProxy;
using System.Collections.Generic;
using System.Reflection;

namespace NetMock
{
	internal class MockProxyAction : IProxyAction
	{
		internal static readonly Func<InvocationContext, Object> DefaultAnswer = (invocation) => {
			var returnType = invocation.Method.ReturnType;
			if (returnType.GetTypeInfo().IsValueType)
			{
				return Activator.CreateInstance(returnType);
			}
			return null;
		};

		private bool stubbingFinalized = true;
		private LinkedList<Func<InvocationContext, Object>> answers = new LinkedList<Func<InvocationContext, object>>();
		private IList<InvocationContext> invocations = new List<InvocationContext>();

		public object OnMethodInvoked(InvocationContext invocation)
		{
			if(!stubbingFinalized)
			{
				throw new InvalidMockUsageException();
			}
			invocations.Add(invocation);
			MockContext.LastInvokedMock = this;
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
			invocations.Clear();
		}

		internal IList<InvocationContext> Invocations { get { return invocations; } }
	}
}