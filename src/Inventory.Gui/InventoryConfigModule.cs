using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using AgileWorkshop.Bus;
using AgileWorkshop.Cqrs.Configuration;
using AgileWorkshop.Cqrs.Core;
using AgileWorkshop.Cqrs.EventStore;
using AgileWorkshop.Cqrs.Reporting;

using Inventory.CommandHandlers;
using Inventory.Commands;
using Inventory.EventHandlers;
using Inventory.Events;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Reflection;
using Ninject.Modules;

namespace Inventory.Gui
{
	public class InventoryConfigModule : NinjectModule
	{
		public override void Load()
		{
			this.Bind<IRouteMessages>().To<MessageRouter>().InSingletonScope();
			this.Bind<ICommandBus>().To<FakeBus>().InRequestScope();
			this.Bind<IEventBus>().To<FakeBus>().InRequestScope();
			this.Bind<IFormatter>().To<BinaryFormatter>().InRequestScope();
			this.Bind<IEventStore>().To<SqliteEventStore>().InRequestScope().WithConstructorArgument(
				"sqLiteConnectionString", "Data Source=eventStore.db3");
			this.Bind(typeof(IDomainRepository<>)).To(typeof(DomainRepository<>)).InRequestScope();

            var commands = HandlerHelper.GetCommands();
            var handlers = HandlerHelper.GetHandlers();
		    var events = HandlerHelper.GetEvents();
            foreach (var command in commands)
            {
                IList<Type> commandHandlerTypes;
                if (!handlers.TryGetValue(command, out commandHandlerTypes))
                    throw new Exception(string.Format("No command handlers found for command '{0}'", command.FullName));

                foreach (var commandHandler in commandHandlerTypes)
                {
                    this.Bind(typeof (IHandle<>).MakeGenericType(command)).To(commandHandler);
                }
            }

		    foreach (var @event in events)
		    {
                IList<Type> commandHandlerTypes;
                if (!handlers.TryGetValue(@event, out commandHandlerTypes))
                    throw new Exception(string.Format("No event handlers found for event '{0}'", @event.FullName));

                foreach (var commandHandler in commandHandlerTypes)
                {
                    this.Bind(typeof(IHandle<>).MakeGenericType(@event)).To(commandHandler);
                }
		    }

            //var handlerList = AssemblyScanner.ScanAssembliesFor(typeof(IHandle<>)).ToList();
            //var commandList = AssemblyScanner.ScanAssembliesFor<Command>().ToList();

            //this.Bind<IHandle<CreateInventoryItem>>().To<InventoryCommandHandlers>().InRequestScope();
            //this.Bind<IHandle<DeactivateInventoryItem>>().To<InventoryCommandHandlers>().InRequestScope();
            //this.Bind<IHandle<RemoveItemsFromInventory>>().To<InventoryCommandHandlers>().InRequestScope();
            //this.Bind<IHandle<CheckInItemsToInventory>>().To<InventoryCommandHandlers>().InRequestScope();
            //this.Bind<IHandle<RenameInventoryItem>>().To<InventoryCommandHandlers>().InRequestScope();

			this.Bind<ISqlInsertBuilder>().To<SqlInsertBuilder>().InRequestScope();
			this.Bind<ISqlSelectBuilder>().To<SqlSelectBuilder>().InRequestScope();
			this.Bind<ISqlUpdateBuilder>().To<SqlUpdateBuilder>().InRequestScope();
			this.Bind<ISqlDeleteBuilder>().To<SqlDeleteBuilder>().InRequestScope();
			this.Bind<ISqlCreateBuilder>().To<SqlCreateBuilder>().InRequestScope();

			this.Bind<IReportingRepository>().To<SQLiteReportingRepository>().InRequestScope().WithConstructorArgument(
				"sqLiteConnectionString", "Data Source=reportingDataBase.db3");

            //this.Bind<IHandle<InventoryItemCreated>>().To<InventoryItemDetailViewHandler>().InRequestScope();
            //this.Bind<IHandle<InventoryItemDeactivated>>().To<InventoryItemDetailViewHandler>().InRequestScope();
            //this.Bind<IHandle<InventoryItemRenamed>>().To<InventoryItemDetailViewHandler>().InRequestScope();
            //this.Bind<IHandle<ItemsRemovedFromInventory>>().To<InventoryItemDetailViewHandler>().InRequestScope();
            //this.Bind<IHandle<ItemsCheckedInToInventory>>().To<InventoryItemDetailViewHandler>().InRequestScope();

            //this.Bind<IHandle<InventoryItemCreated>>().To<InventoryItemListViewHandler>().InRequestScope();
            //this.Bind<IHandle<InventoryItemRenamed>>().To<InventoryItemListViewHandler>().InRequestScope();
            //this.Bind<IHandle<InventoryItemDeactivated>>().To<InventoryItemListViewHandler>().InRequestScope();
		}
	}
}