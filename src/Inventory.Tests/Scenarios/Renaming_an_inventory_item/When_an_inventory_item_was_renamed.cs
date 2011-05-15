using System;
using System.Text;
using Inventory.EventHandlers;
using Inventory.Events;
using Inventory.Reporting;
using Inventory.Reporting.Dto;
using Moq;

namespace Inventory.Tests.Scenarios.Adding_a_new_inventory_item
{
    public class When_an_inventory_item_was_renamed : EventTestFixture<InventoryItemRenamed, InventoryItemDetailViewHandler>
    {
            private static Guid _itemId;
            private object UpdateItemObject;
            private object WhereItemObject;

            protected override void SetupDependencies()
            {
                OnDependency<IReportingRepository>()
                    .Setup(x => x.Update<InventoryItemDetailsDto>(It.IsAny<object>(), It.IsAny<object>()))
                    .Callback<object, object>((u, w) => { UpdateItemObject = u; WhereItemObject = w; });
            }

            protected override InventoryItemRenamed When()
            {
                var inventoryItemCreated = new InventoryItemRenamed(Guid.NewGuid(), "Renamed Item Name");
                _itemId = inventoryItemCreated.Id;
                return inventoryItemCreated;
            }

            [Then]
            public void Then_the_reporting_repository_will_be_used_to_update_the_inventory_item_details_report()
            {
                OnDependency<IReportingRepository>().Verify(x => x.Update<InventoryItemDetailsDto>(It.IsAny<object>(), It.IsAny<object>()));
            }

            [Then]
            public void Then_the_inventory_item_details_report_will_be_updated_with_the_expected_details()
            {
                UpdateItemObject.WillBeSimuliar(new {Name = "Renamed Item Name", Version = 0});
                WhereItemObject.WillBeSimuliar(new { Id = _itemId });
            }        
    }

    public class When_an_inventory_item_was_renamed2 : EventTestFixture<InventoryItemRenamed, InventoryItemListViewHandler>
    {
        private static Guid _itemId;
        private object UpdateItemObject;
        private object WhereItemObject;

        protected override void SetupDependencies()
        {
            OnDependency<IReportingRepository>()
                .Setup(x => x.Update<InventoryItemListDto>(It.IsAny<object>(), It.IsAny<object>()))
                .Callback<object, object>((u, w) => { UpdateItemObject = u; WhereItemObject = w; });
        }

        protected override InventoryItemRenamed When()
        {
            var inventoryItemCreated = new InventoryItemRenamed(Guid.NewGuid(), "Renamed Item Name");
            _itemId = inventoryItemCreated.Id;
            return inventoryItemCreated;
        }

        [Then]
        public void Then_the_reporting_repository_will_be_used_to_update_the_inventory_item_details_report()
        {
            OnDependency<IReportingRepository>().Verify(x => x.Update<InventoryItemListDto>(It.IsAny<object>(), It.IsAny<object>()));
        }

        [Then]
        public void Then_the_inventory_item_details_report_will_be_updated_with_the_expected_details()
        {
            UpdateItemObject.WillBeSimuliar(new { Name = "Renamed Item Name" });
            WhereItemObject.WillBeSimuliar(new { Id = _itemId });
        }
    }
}
