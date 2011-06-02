using System;

using AgileWorkshop.Cqrs.NServiceBus;

using Inventory.Events;
using Inventory.NServiceBus.EventPublisherMessages;

using NServiceBus;

namespace Inventory.NServiceBus.EventPublisherTest
{
	public class Runnner : IWantToRunAtStartup
	{

		public IBus Bus { get; set; }

		public void Run()
		{
			Bus.Publish(
					new TestPublishMessage { RealEvent = new InventoryItemCreated(Guid.NewGuid(), "Dave") });

			while (Console.ReadLine() != "q")
			{
				Bus.Publish(
					new TestPublishMessage { RealEvent = new InventoryItemCreated(Guid.NewGuid(), "Dave2") });

				Bus.Publish(
					new NServiceBusEventMessage<InventoryItemCreated> { RealEvent = new InventoryItemCreated(Guid.NewGuid(), "Dave3") });
			}
		}

		public void Stop()
		{
			throw new NotImplementedException();
		}
	}
}