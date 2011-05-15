using System;
using AgileWorkshop.Cqrs.Core;

namespace Inventory.Events
{
	[Serializable]
	public class ItemsCheckedInToInventory : Event
	{
		public Guid Id { get; set; }
		public int Count { get; set; }

		public ItemsCheckedInToInventory(Guid id, int count) {
			Id = id;
			Count = count;
		}
	}
}