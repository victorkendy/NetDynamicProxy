using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetMock
{
	internal class MockContext
	{
		private static readonly ThreadLocal<MockProxyAction> lastInvockedMocks = new ThreadLocal<MockProxyAction>();

		internal static MockProxyAction LastInvokedMock
		{
			get
			{
				return lastInvockedMocks.Value;
			}
			set
			{
				lastInvockedMocks.Value = value;
			}
		}
	}
}
