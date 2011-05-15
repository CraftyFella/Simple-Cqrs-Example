using System;
using AgileWorkshop.Cqrs.Core;

namespace Inventory.Commands
{
	[Serializable]
	public class RenameInventoryItem : Command {
		public Guid InventoryItemId { get; private set; }
		public string NewName { get; private set; }
		public int OriginalVersion { get; private set; }

		public RenameInventoryItem(Guid inventoryItemId, string newName, int originalVersion)
		{
			InventoryItemId = inventoryItemId;
			NewName = newName;
			OriginalVersion = originalVersion;
		}
	}
}