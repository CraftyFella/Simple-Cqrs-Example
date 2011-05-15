using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.Reporting.Dto;

namespace Inventory.Reporting
{
	public class ReadModelFacade : IReadModelFacade
	{
		private readonly IReportingRepository reportingRepository;

		public ReadModelFacade(IReportingRepository reportingRepository)
		{
			this.reportingRepository = reportingRepository;
		}

		public IEnumerable<InventoryItemListDto> GetInventoryItems()
		{
			return reportingRepository.GetByExample<InventoryItemListDto>(null).ToList().ToList();
		}

		public InventoryItemDetailsDto GetInventoryItemDetails(Guid id)
		{
			return reportingRepository.GetByExample<InventoryItemDetailsDto>(new { Id = id }).FirstOrDefault();
		}
	}
}