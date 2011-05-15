using System;
using AgileWorkshop.Cqrs.Core;

namespace Inventory.Commands
{
	[Serializable]
	public class CreateInventoryItem : Command {
		public Guid InventoryItemId { get; private set; }
		public string Name { get; private set; }

		public CreateInventoryItem(Guid inventoryItemId, string name)
		{
			InventoryItemId = inventoryItemId;
			Name = name;
		}
	}
}