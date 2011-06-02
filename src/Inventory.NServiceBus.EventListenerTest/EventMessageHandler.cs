using System;

using AgileWorkshop.Cqrs.NServiceBus;

using Inventory.Events;
using Inventory.NServiceBus.EventPublisherMessages;

using NServiceBus;

namespace Inventory.NServiceBus.EventListenerTest
{
	public class EventMessageHandler : IHandleMessages<TestPublishMessage>
	{

		public void Handle(TestPublishMessage message)
		{
			Console.WriteLine("Hello");
		}
	}

	public class EventMessageHandler2 : IHandleMessages<NServiceBusEventMessage<InventoryItemCreated>>
	{

		public void Handle(NServiceBusEventMessage<InventoryItemCreated> message)
		{
			Console.WriteLine("Hello2");
		}
	}
}