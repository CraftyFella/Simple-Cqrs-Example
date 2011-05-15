using System;
using System.Collections.Generic;
using System.Linq;
using AgileWorkshop.Cqrs.Core;
using Inventory.CommandHandlers;
using Inventory.Commands;
using Inventory.Domain;
using Inventory.Events;
using Inventory.EventStore;
using Moq;

namespace Inventory.Tests.Scenarios.Adding_a_new_inventory_item
{
	public class When_creating_a_new_inventory_item : CommandTestFixture<CreateInventoryItem, InventoryCommandHandlers, InventoryItem>
	{
		protected override CreateInventoryItem When()
		{
			return new CreateInventoryItem(Guid.NewGuid(), "Dave");
		}


        [Then]
        public void Then_an_inventory_item_created_event_will_be_published()
        {
            PublishedEvents.Last().WillBeOfType<InventoryItemCreated>();
        }

        [Then]
        public void Then_the_published_event_will_contain_the_name_of_the_item()
        {
            PublishedEvents.Last<InventoryItemCreated>().Name.WillBe("Dave");
        }

	}
}
