using System;
using AgileWorkshop.Cqrs.Core;

namespace Inventory.Commands
{
	[Serializable]
	public class DeactivateInventoryItem : Command {
		public Guid InventoryItemId { get; private set; }
		public int OriginalVersion { get; private set; }

		public DeactivateInventoryItem(Guid inventoryItemId, int originalVersion)
		{
			InventoryItemId = inventoryItemId;
			OriginalVersion = originalVersion;
		}
	}
}