using System;
using AgileWorkshop.Cqrs.Core;

namespace Inventory.Events
{
	[Serializable]
	public class InventoryItemDeactivated : Event {
		public Guid Id { get; set; }

		public InventoryItemDeactivated(Guid id)
		{
			Id = id;
		}
	}
}