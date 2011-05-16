using System;
using System.Collections.Generic;
using System.Linq;
using AgileWorkshop.Cqrs.Core;
using Inventory.CommandHandlers;
using Inventory.Commands;
using Inventory.Domain;
using Inventory.Events;

using Moq;

namespace Inventory.Tests.Scenarios.Adding_a_new_inventory_item
{
    public class When_renaming_an_existing_inventory_item : CommandTestFixture<RenameInventoryItem, InventoryCommandHandlers, InventoryItem>
	{
        protected override IEnumerable<Event> Given()
        {
            yield return new InventoryItemCreated(Guid.NewGuid(), "Dave");
        }

        protected override RenameInventoryItem When()
		{
            return new RenameInventoryItem(Guid.NewGuid(), "Dave 2", 1);
		}

        [Then]
        public void Then_an_inventory_item_renamed_event_will_be_published()
        {
            PublishedEvents.Last().WillBeOfType<InventoryItemRenamed>();
        }

        [Then]
        public void Then_the_published_event_will_contain_the_name_of_the_item()
        {
            PublishedEvents.Last<InventoryItemRenamed>().NewName.WillBe("Dave 2");
        }

	}
}
