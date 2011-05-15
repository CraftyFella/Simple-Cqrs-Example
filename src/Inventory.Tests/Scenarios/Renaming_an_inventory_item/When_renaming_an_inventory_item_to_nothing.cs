using System;
using System.Collections.Generic;
using System.Linq;
using AgileWorkshop.Cqrs.Core;
using Inventory.CommandHandlers;
using Inventory.Commands;
using Inventory.Domain;
using Inventory.Events;

namespace Inventory.Tests.Scenarios.Adding_a_new_inventory_item
{
    public class When_renaming_an_inventory_item_to_nothing : CommandTestFixture<RenameInventoryItem, InventoryCommandHandlers, InventoryItem>
	{
        protected override IEnumerable<Event> Given()
        {
            yield return new InventoryItemCreated(Guid.NewGuid(), "Dave");
        }

        protected override RenameInventoryItem When()
		{
            return new RenameInventoryItem(Guid.NewGuid(), string.Empty, 1);
		}

        [Then]
        public void Then_an_argument_exception_will_be_thrown()
        {
            CaughtException.WillBeOfType<ArgumentException>();
        }

        [Then]
        public void Then_the_exception_message_will_be()
        {
            CaughtException.Message.WillBe("newName");
        }

	}
}
