using System;
using System.Text;
using Inventory.EventHandlers;
using Inventory.Events;
using Inventory.Reporting;
using Inventory.Reporting.Dto;
using Moq;

namespace Inventory.Tests.Scenarios.Adding_a_new_inventory_item
{
	using AgileWorkshop.Cqrs.Reporting;

	public class When_an_inventory_item_was_added : EventTestFixture<InventoryItemCreated, InventoryItemDetailViewHandler>
    {
            private static Guid _itemId;
            private InventoryItemDetailsDto SaveItemObject;

            protected override void SetupDependencies()
            {
                OnDependency<IReportingRepository>()
                    .Setup(x => x.Save<InventoryItemDetailsDto>(It.IsAny<InventoryItemDetailsDto>()))
                    .Callback<InventoryItemDetailsDto>((s) => { SaveItemObject = s; });
            }

            protected override InventoryItemCreated When()
            {
                var inventoryItemCreated = new InventoryItemCreated(Guid.NewGuid(), "New Item Name");
                _itemId = inventoryItemCreated.Id;
                return inventoryItemCreated;
            }

            [Then]
            public void Then_the_reporting_repository_will_be_used_to_update_the_inventory_item_details_report()
            {
                OnDependency<IReportingRepository>().Verify(x => x.Save<InventoryItemDetailsDto>(It.IsAny<InventoryItemDetailsDto>()));
            }

            [Then]
            public void Then_the_inventory_item_details_report_will_be_updated_with_the_expected_details()
            {
                SaveItemObject.Id.WillBe(_itemId);
                SaveItemObject.Name.WillBe("New Item Name");
                SaveItemObject.CurrentCount.WillBe(0);
                SaveItemObject.Version.WillBe(0);
            }        
    }

    public class When_an_inventory_item_was_added2 : EventTestFixture<InventoryItemCreated, InventoryItemListViewHandler>
    {
        private static Guid _itemId;
        private InventoryItemListDto SaveItemObject;

        protected override void SetupDependencies()
        {
            OnDependency<IReportingRepository>()
                .Setup(x => x.Save<InventoryItemListDto>(It.IsAny<InventoryItemListDto>()))
                .Callback<InventoryItemListDto>((s) => { SaveItemObject = s; });
        }

        protected override InventoryItemCreated When()
        {
            var inventoryItemCreated = new InventoryItemCreated(Guid.NewGuid(), "New Item Name");
            _itemId = inventoryItemCreated.Id;
            return inventoryItemCreated;
        }

        [Then]
        public void Then_the_reporting_repository_will_be_used_to_update_the_inventory_item_list_report()
        {
            OnDependency<IReportingRepository>().Verify(x => x.Save<InventoryItemListDto>(It.IsAny<InventoryItemListDto>()));
        }

        [Then]
        public void Then_the_inventory_item_list_report_will_be_updated_with_the_expected_details()
        {
            SaveItemObject.Id.WillBe(_itemId);
            SaveItemObject.Name.WillBe("New Item Name");
        }
    }
}
