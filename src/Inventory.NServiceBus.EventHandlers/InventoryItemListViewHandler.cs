using System;

using AgileWorkshop.Bus;

using Inventory.Events;

namespace Inventory.NServiceBus.EventHandlers
{
    public class InventoryItemListViewHandler : IHandle<InventoryItemCreated>, IHandle<InventoryItemRenamed>, IHandle<InventoryItemDeactivated>, IHandle<ItemsCheckedInToInventory>, IHandle<ItemsRemovedFromInventory>
	{
		public void Handle(InventoryItemCreated message)
		{
			Console.WriteLine("InventoryItemCreated");
		}

		public void Handle(InventoryItemRenamed message)
		{
			Console.WriteLine("InventoryItemRenamed");
		}

		public void Handle(InventoryItemDeactivated message)
		{
			Console.WriteLine("InventoryItemDeactivated");
		}

        public void Handle(ItemsCheckedInToInventory message)
        {
            Console.WriteLine("ItemsCheckedInToInventory");
        }

        public void Handle(ItemsRemovedFromInventory message)
        {
            Console.WriteLine("ItemsRemovedFromInventory");
        }
	}
}