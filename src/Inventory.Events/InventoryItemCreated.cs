using System;
using AgileWorkshop.Cqrs.Core;

namespace Inventory.Events
{
	[Serializable]
	public class InventoryItemCreated : Event {
		public Guid Id { get; set; }
		public string Name { get; set; }

		public InventoryItemCreated(Guid id, string name) {
			Id = id;
			Name = name;
		}
	}
}