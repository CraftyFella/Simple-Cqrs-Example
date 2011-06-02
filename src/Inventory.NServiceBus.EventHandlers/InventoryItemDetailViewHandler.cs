using System;

using AgileWorkshop.Bus;

using Inventory.Events;

namespace Inventory.NServiceBus.EventHandlers
{
	public class InventoryItemDetailViewHandler : IHandle<InventoryItemCreated>, IHandle<InventoryItemDeactivated>, IHandle<InventoryItemRenamed>, IHandle<ItemsRemovedFromInventory>, IHandle<ItemsCheckedInToInventory>
	{
		public void Handle(InventoryItemCreated message)
		{
			Console.WriteLine("InventoryItemCreated");
		}

		public void Handle(InventoryItemRenamed message)
		{
			Console.WriteLine("InventoryItemRenamed");	
		}

		public void Handle(ItemsRemovedFromInventory message)
		{
			Console.WriteLine("ItemsRemovedFromInventory");
		}

		public void Handle(ItemsCheckedInToInventory message)
		{
			Console.WriteLine("ItemsCheckedInToInventory");
		}

		public void Handle(InventoryItemDeactivated message)
		{
			Console.WriteLine("InventoryItemDeactivated");
		}
	}
}