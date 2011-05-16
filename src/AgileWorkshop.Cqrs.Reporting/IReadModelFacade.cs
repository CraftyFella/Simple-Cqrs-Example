namespace AgileWorkshop.Cqrs.Reporting
{
	using System;
	using System.Collections.Generic;

	using Inventory.Reporting.Dto;

	public interface IReadModelFacade
	{
		IEnumerable<InventoryItemListDto> GetInventoryItems();
		InventoryItemDetailsDto GetInventoryItemDetails(Guid id);
	}
}