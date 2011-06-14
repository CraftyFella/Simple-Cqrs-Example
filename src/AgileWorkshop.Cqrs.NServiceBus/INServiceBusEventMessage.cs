using System.Collections.Generic;
using AgileWorkshop.Cqrs.Core;

using NServiceBus;

namespace AgileWorkshop.Cqrs.NServiceBus
{
	public interface INServiceBusEventMessage : IMessage
	{
		Event RealEvent { get; set; }
		string Header { get; set; }
	}
}