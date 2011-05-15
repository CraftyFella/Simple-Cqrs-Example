using AgileWorkshop.Bus;
using AgileWorkshop.Cqrs.Core;
using Inventory.Events;
using Inventory.Reporting;
using Inventory.Reporting.Dto;

namespace Inventory.EventHandlers
{
	public class InventoryItemListViewHandler : IHandle<InventoryItemCreated>, IHandle<InventoryItemRenamed>, IHandle<InventoryItemDeactivated>
	{
		private readonly IReportingRepository reportingRepository;

		public InventoryItemListViewHandler(IReportingRepository reportingRepository)
		{
			this.reportingRepository = reportingRepository;
		}

		public void Handle(InventoryItemCreated message)
		{
			reportingRepository.Save(new InventoryItemListDto(message.Id, message.Name));
		}

		public void Handle(InventoryItemRenamed message)
		{
			reportingRepository.Update<InventoryItemListDto>(new { Name = message.NewName }, new { Id = message.Id });
		}

		public void Handle(InventoryItemDeactivated message)
		{
			reportingRepository.Delete<InventoryItemListDto>(new { Id = message.Id });
		}
	}
}