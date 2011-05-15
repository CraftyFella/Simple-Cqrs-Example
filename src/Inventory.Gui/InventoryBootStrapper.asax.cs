using System.Runtime.Serialization.Formatters.Binary;
using AgileWorkshop.Cqrs.Core;
using Inventory.CommandHandlers;
using Inventory.Commands;
using Inventory.Domain;
using Inventory.EventHandlers;
using Inventory.Events;
using Inventory.EventStore;
using Inventory.Reporting;

namespace Inventory.Gui
{
    public class InventoryBootStrapper
    {
        public void BootStrapTheApplication()
        {
            EventStoreDatabaseBootStrapper.BootStrap();
            ReportingDatabaseBootStrapper.BootStrap();

            var bus = new FakeBus();
            ServiceLocator.Bus = bus;
            //var storage = new InMemoryEventStore(bus);
            //var storage = new NHibernateSqliteEventStore(bus, NHibernateSessionHelper.CreateSessionFactory(), new BinaryFormatter());
            var storage = new SqliteEventStore(bus, new BinaryFormatter(), "Data Source=eventStore.db3");

            var rep = new DomainRepository<InventoryItem>(storage);
            var commands = new InventoryCommandHandlers(rep);
            bus.RegisterHandler<CheckInItemsToInventory>(commands.Handle);
            bus.RegisterHandler<CreateInventoryItem>(commands.Handle);
            bus.RegisterHandler<DeactivateInventoryItem>(commands.Handle);
            bus.RegisterHandler<RemoveItemsFromInventory>(commands.Handle);
            bus.RegisterHandler<RenameInventoryItem>(commands.Handle);

            var sqlInsertBuilder = new SqlInsertBuilder();
            var sqlSelectBuilder = new SqlSelectBuilder();
            var sqlUpdateBuilder = new SqlUpdateBuilder();
            var sqlDeleteBuilder = new SqlDeleteBuilder();
            var reportingRepository = new SQLiteReportingRepository("Data Source=reportingDataBase.db3", sqlSelectBuilder,
                                                                    sqlInsertBuilder, sqlUpdateBuilder, sqlDeleteBuilder);
            var detail = new InventoryItemDetailViewHandler(reportingRepository);
            ServiceLocator.ReportingRepository = reportingRepository;
            bus.RegisterHandler<InventoryItemCreated>(detail.Handle);
            bus.RegisterHandler<InventoryItemDeactivated>(detail.Handle);
            bus.RegisterHandler<InventoryItemRenamed>(detail.Handle);
            bus.RegisterHandler<ItemsCheckedInToInventory>(detail.Handle);
            bus.RegisterHandler<ItemsRemovedFromInventory>(detail.Handle);
            var list = new InventoryItemListViewHandler(reportingRepository);
            bus.RegisterHandler<InventoryItemCreated>(list.Handle);
            bus.RegisterHandler<InventoryItemRenamed>(list.Handle);
            bus.RegisterHandler<InventoryItemDeactivated>(list.Handle);
            
        }

        public static void BootStrap()
        {
            new InventoryBootStrapper().BootStrapTheApplication();
        }
    }
}