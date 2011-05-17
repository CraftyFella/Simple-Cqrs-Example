using System.Runtime.Serialization.Formatters.Binary;
using AgileWorkshop.Bus;
using AgileWorkshop.Cqrs.Core;
using Inventory.CommandHandlers;
using Inventory.Commands;
using Inventory.Domain;
using Inventory.EventHandlers;
using Inventory.Events;
using Inventory.Reporting;

namespace Inventory.Gui
{
	using System.Runtime.Serialization;

	using AgileWorkshop.Cqrs.EventStore;
	using AgileWorkshop.Cqrs.Reporting;

	using Ninject;
	using Ninject.Modules;

	public class InventoryBootStrapper
    {
        public void BootStrapTheApplication()
        {
			EventStoreDatabaseBootStrapper.BootStrap();
            ReportingDatabaseBootStrapper.BootStrap();
			//kernel = new StandardKernel(new InventoryConfigModule());

			//var router = kernel.Get<IRouteMessages>();

			//// Register Command Handlers
			//router.RegisterHandler<CheckInItemsToInventory>(kernel.Get<IHandle<CheckInItemsToInventory>>().Handle);
			//router.RegisterHandler<CreateInventoryItem>(kernel.Get<IHandle<CreateInventoryItem>>().Handle);
			//router.RegisterHandler<DeactivateInventoryItem>(kernel.Get<IHandle<DeactivateInventoryItem>>().Handle);
			//router.RegisterHandler<RemoveItemsFromInventory>(kernel.Get<IHandle<RemoveItemsFromInventory>>().Handle);
			//router.RegisterHandler<RenameInventoryItem>(kernel.Get<IHandle<RenameInventoryItem>>().Handle);

			//// Register Event Handlers
			//router.RegisterHandler<InventoryItemCreated>(kernel.Get<IHandle<InventoryItemCreated>>().Handle);
			//router.RegisterHandler<InventoryItemDeactivated>(kernel.Get<IHandle<InventoryItemDeactivated>>().Handle);
			//router.RegisterHandler<InventoryItemRenamed>(kernel.Get<IHandle<InventoryItemRenamed>>().Handle);
			//router.RegisterHandler<ItemsCheckedInToInventory>(kernel.Get<IHandle<ItemsCheckedInToInventory>>().Handle);
			//router.RegisterHandler<ItemsRemovedFromInventory>(kernel.Get<IHandle<ItemsRemovedFromInventory>>().Handle);

			//router.RegisterHandler<InventoryItemCreated>(kernel.Get<IHandle<InventoryItemCreated>>().Handle);
			//router.RegisterHandler<InventoryItemRenamed>(kernel.Get<IHandle<InventoryItemRenamed>>().Handle);
			//router.RegisterHandler<InventoryItemDeactivated>(kernel.Get<IHandle<InventoryItemDeactivated>>().Handle);
			
        }

        public static void BootStrap()
        {
            new InventoryBootStrapper().BootStrapTheApplication();
        }

		
    }

	
}