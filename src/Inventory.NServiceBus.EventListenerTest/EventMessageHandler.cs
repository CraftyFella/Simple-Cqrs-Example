using System;

using AgileWorkshop.Cqrs.NServiceBus;

using Inventory.Events;

using NServiceBus;

namespace Inventory.NServiceBus.EventListenerTest
{
	public class EventMessageHandler : IHandleMessages<NServiceBusEventMessage<InventoryItemCreated>>
	{

		public void Handle(NServiceBusEventMessage<InventoryItemCreated> message)
		{
			Console.WriteLine("Hello2");
		}
	}
}