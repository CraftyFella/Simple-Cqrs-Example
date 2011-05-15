using System;

namespace Inventory.Reporting.Dto
{
	public class InventoryItemListDto
	{
		public Guid Id;
		public string Name;

		public InventoryItemListDto(Guid id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}