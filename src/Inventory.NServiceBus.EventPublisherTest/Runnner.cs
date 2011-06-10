using System;

using AgileWorkshop.Cqrs.NServiceBus;

using Inventory.Events;

using NServiceBus;

namespace Inventory.NServiceBus.EventPublisherTest
{
	public class Runnner : IWantToRunAtStartup
	{

		public IBus Bus { get; set; }

		public void Run()
		{
			while (Console.ReadLine() != "q")
			{
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