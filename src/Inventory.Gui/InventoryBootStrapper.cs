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
	using AgileWorkshop.Cqrs.EventStore;
	using AgileWorkshop.Cqrs.Reporting;

	public class InventoryBootStrapper
    {
        public void BootStrapTheApplication()
        {
            EventStoreDatabaseBootStrapper.BootStrap();
            ReportingDatabaseBootStrapper.BootStrap();

			var router = new MessageRouter();
			var commandBus = new FakeBus(router);
			var eventBus = new FakeBus(router);

            ServiceLocator.CommandBus = commandBus;
			ServiceLocator.EventBus = eventBus;

            //var storage = new InMemoryEventStore(bus);
            //var storage = new NHibernateSqliteEventStore(bus, NHibernateSessionHelper.CreateSessionFactory(), new BinaryFormatter());
            var storage = new SqliteEventStore(ServiceLocator.EventBus, new BinaryFormatter(), "Data Source=eventStore.db3");

            var rep = new DomainRepository<InventoryItem>(storage);
            var commands = new InventoryCommandHandlers(rep);
            router.RegisterHandler<CheckInItemsToInventory>(commands.Handle);
            router.RegisterHandler<CreateInventoryItem>(commands.Handle);
            router.RegisterHandler<DeactivateInventoryItem>(commands.Handle);
            router.RegisterHandler<RemoveItemsFromInventory>(commands.Handle);
            router.RegisterHandler<RenameInventoryItem>(commands.Handle);

            var sqlInsertBuilder = new SqlInsertBuilder();
            var sqlSelectBuilder = new SqlSelectBuilder();
            var sqlUpdateBuilder = new SqlUpdateBuilder();
            var sqlDeleteBuilder = new SqlDeleteBuilder();
            var reportingRepository = new SQLiteReportingRepository("Data Source=reportingDataBase.db3", sqlSelectBuilder,
                                                                    sqlInsertBuilder, sqlUpdateBuilder, sqlDeleteBuilder);
            var detail = new InventoryItemDetailViewHandler(reportingRepository);
            ServiceLocator.ReportingRepository = reportingRepository;
            router.RegisterHandler<InventoryItemCreated>(detail.Handle);
            router.RegisterHandler<InventoryItemDeactivated>(detail.Handle);
            router.RegisterHandler<InventoryItemRenamed>(detail.Handle);
            router.RegisterHandler<ItemsCheckedInToInventory>(detail.Handle);
            router.RegisterHandler<ItemsRemovedFromInventory>(detail.Handle);
            var list = new InventoryItemListViewHandler(reportingRepository);
            router.RegisterHandler<InventoryItemCreated>(list.Handle);
            router.RegisterHandler<InventoryItemRenamed>(list.Handle);
            router.RegisterHandler<InventoryItemDeactivated>(list.Handle);
            
        }

        public static void BootStrap()
        {
            new InventoryBootStrapper().BootStrapTheApplication();
        }
    }
}