using System;
using AgileWorkshop.Cqrs.Core;

namespace Inventory.Events
{
	[Serializable]
	public class ItemsRemovedFromInventory : Event
	{
		public Guid Id { get; set; }
		public int Count { get; set; }

		public ItemsRemovedFromInventory(Guid id, int count) {
			Id = id;
			Count = count;
		}
	}
}