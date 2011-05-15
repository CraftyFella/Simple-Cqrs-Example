using System;
using System.Collections.Generic;
using Inventory.Reporting.Dto;

namespace Inventory.Reporting
{
	public interface IReadModelFacade
	{
		IEnumerable<InventoryItemListDto> GetInventoryItems();
		InventoryItemDetailsDto GetInventoryItemDetails(Guid id);
	}
}