using System;
using System.Linq;
using AgileWorkshop.Cqrs.Core;
using Inventory.Events;
using Inventory.Reporting;
using Inventory.Reporting.Dto;

namespace Inventory.EventHandlers
{
	public class InventoryItemDetailViewHandler : IHandle<InventoryItemCreated>, IHandle<InventoryItemDeactivated>, IHandle<InventoryItemRenamed>, IHandle<ItemsRemovedFromInventory>, IHandle<ItemsCheckedInToInventory>
	{
		private readonly IReportingRepository reportingRepository;

		public InventoryItemDetailViewHandler(IReportingRepository reportingRepository)
		{
			this.reportingRepository = reportingRepository;
		}

		public void Handle(InventoryItemCreated message)
		{
			reportingRepository.Save(new InventoryItemDetailsDto(message.Id, message.Name, 0, 0));
		}

		public void Handle(InventoryItemRenamed message)
		{
			reportingRepository.Update<InventoryItemDetailsDto>(new { Name =  message.NewName, Version = message.Version }, new { Id = message.Id });
		}

		public void Handle(ItemsRemovedFromInventory message)
		{
			var d = GetDetailsItem(message.Id);
			reportingRepository.Update<InventoryItemDetailsDto>(new { CurrentCount = d.CurrentCount -= message.Count, Version = message.Version }, new { Id = message.Id });
		}

		public void Handle(ItemsCheckedInToInventory message)
		{
			var d = GetDetailsItem(message.Id);
			reportingRepository.Update<InventoryItemDetailsDto>(new { CurrentCount = d.CurrentCount += message.Count, Version = message.Version }, new { Id = message.Id });
		}

		private InventoryItemDetailsDto GetDetailsItem(Guid id)
		{
			return reportingRepository.GetByExample<InventoryItemDetailsDto>(new {Id = id}).FirstOrDefault();
		}

		public void Handle(InventoryItemDeactivated message)
		{
			reportingRepository.Delete<InventoryItemDetailsDto>(new { Id = message.Id });
		}
	}
}