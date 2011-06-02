
using System;

using AgileWorkshop.Cqrs.NServiceBus;

using Inventory.Events;

namespace Inventory.NServiceBus.EventPublisherMessages
{
	[Serializable]
	public class TestPublishMessage : NServiceBusEventMessage<InventoryItemCreated>
	{

	}
}
