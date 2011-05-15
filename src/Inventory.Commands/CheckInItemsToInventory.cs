﻿using System;
using AgileWorkshop.Cqrs.Core;

namespace Inventory.Commands
{
	[Serializable]
	public class CheckInItemsToInventory : Command {
		public Guid InventoryItemId { get; private set; }
		public int Count { get; private set; }
		public int OriginalVersion { get; private set; }

		public CheckInItemsToInventory(Guid inventoryItemId, int count, int originalVersion) {
			InventoryItemId = inventoryItemId;
			Count = count;
			OriginalVersion = originalVersion;
		}
	}
}