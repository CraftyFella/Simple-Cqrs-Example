using AgileWorkshop.Bus;
using Inventory.Commands;
using Inventory.Events;
using AgileWorkshop.Cqrs.EventStore;
using AgileWorkshop.Cqrs.Reporting;
using Ninject;

namespace Inventory.Gui
{
	

	public class InventoryBootStrapper
    {
        public void BootStrapTheApplication()
        {
			EventStoreDatabaseBootStrapper.BootStrap();
            ReportingDatabaseBootStrapper.BootStrap();

			var router = MvcApplication.InventoryKernal.Get<IRouteMessages>();

			// TODO: Tidy up into a Helper (Get all IHandles and register with Router

			// Register Command Handlers
			router.RegisterHandler<CheckInItemsToInventory>(MvcApplication.InventoryKernal.Get<IHandle<CheckInItemsToInventory>>().Handle);
			router.RegisterHandler<CreateInventoryItem>(MvcApplication.InventoryKernal.Get<IHandle<CreateInventoryItem>>().Handle);
			router.RegisterHandler<DeactivateInventoryItem>(MvcApplication.InventoryKernal.Get<IHandle<DeactivateInventoryItem>>().Handle);
			router.RegisterHandler<RemoveItemsFromInventory>(MvcApplication.InventoryKernal.Get<IHandle<RemoveItemsFromInventory>>().Handle);
			router.RegisterHandler<RenameInventoryItem>(MvcApplication.InventoryKernal.Get<IHandle<RenameInventoryItem>>().Handle);

			// Register Event Handlers
			foreach (var handler in MvcApplication.InventoryKernal.GetAll<IHandle<InventoryItemCreated>>())
				router.RegisterHandler<InventoryItemCreated>(handler.Handle);
			
			foreach (var handler in MvcApplication.InventoryKernal.GetAll<IHandle<InventoryItemRenamed>>())
				router.RegisterHandler<InventoryItemRenamed>(handler.Handle);

			foreach (var handler in MvcApplication.InventoryKernal.GetAll<IHandle<InventoryItemDeactivated>>())
				router.RegisterHandler<InventoryItemDeactivated>(handler.Handle);

			router.RegisterHandler<ItemsCheckedInToInventory>(MvcApplication.InventoryKernal.Get<IHandle<ItemsCheckedInToInventory>>().Handle);
			router.RegisterHandler<ItemsRemovedFromInventory>(MvcApplication.InventoryKernal.Get<IHandle<ItemsRemovedFromInventory>>().Handle);
        }

        public static void BootStrap()
        {
            new InventoryBootStrapper().BootStrapTheApplication();
        }

		
    }

	
}