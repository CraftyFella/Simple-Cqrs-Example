using System;
using System.Linq;
using Inventory.CommandHandlers;
using Inventory.Commands;
using Inventory.Domain;
using Inventory.Events;
using NUnit.Framework;

namespace Inventory.Tests.Scenarios.Adding_a_new_inventory_item
{
    public class When_renaming_an_non_existing_inventory_item : CommandTestFixture<RenameInventoryItem, InventoryCommandHandlers, InventoryItem>
	{
        protected override RenameInventoryItem When()
		{
            return new RenameInventoryItem(Guid.NewGuid(), "Dave 2", 1);
		}

        [Then]
        [Ignore("Come back to this, when I've worked out where to throw the exception")]
        public void Then_a_non_existing_item_exception_will_be_thrown()
        {
            //CaughtException.WillBeOfType<NonExitsingInventoryItemException>();
        }

        [Then]
        [Ignore("Come back to this, when I've worked out where to throw the exception")]
        public void Then_the_exception_message_will_be()
        {
            CaughtException.Message.WillBe("The Inventory Item cannot be found");
        }

	}
}
