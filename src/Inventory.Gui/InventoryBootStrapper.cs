using System;
using System.Collections.Generic;
using System.Reflection;
using AgileWorkshop.Bus;
using AgileWorkshop.Cqrs.Configuration;
using AgileWorkshop.Cqrs.EventStore;
using AgileWorkshop.Cqrs.Reporting;
using Ninject;

namespace Inventory.Gui
{
	

	public class InventoryBootStrapper
    {
	    private MethodInfo _createActionMethod;
	    private static MethodInfo _registerMethod;

	    public void BootStrapTheApplication()
        {
			EventStoreDatabaseBootStrapper.BootStrap();
            ReportingDatabaseBootStrapper.BootStrap();

			var router = MvcApplication.InventoryKernal.Get<IRouteMessages>();

            _createActionMethod = GetType().GetMethod("CreateAction");
            _registerMethod = router.GetType().GetMethod("RegisterHandler");

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
                    var injectedHandler = GetCorrectlyInjectedHandler(commandHandler);
                    var action = CreateTheProperAction(command, injectedHandler);
                    RegisterTheCreatedActionWithTheMessageRouter(router, command, action);
                }
            }

            foreach (var @event in events)
            {
                IList<Type> commandHandlerTypes;
                if (!handlers.TryGetValue(@event, out commandHandlerTypes))
                    throw new Exception(string.Format("No event handlers found for event '{0}'", @event.FullName));

                foreach (var commandHandler in commandHandlerTypes)
                {
                    var injectedHandler = GetCorrectlyInjectedHandler(commandHandler);
                    var action = CreateTheProperAction(@event, injectedHandler);
                    RegisterTheCreatedActionWithTheMessageRouter(router, @event, action);
                }
            }

            // TODO: Tidy up into a Helper (Get all IHandles and register with Router)
            // Register Command Handlers
            //router.RegisterHandler<CheckInItemsToInventory>(MvcApplication.InventoryKernal.Get<IHandle<CheckInItemsToInventory>>().Handle);
            //router.RegisterHandler<CreateInventoryItem>(MvcApplication.InventoryKernal.Get<IHandle<CreateInventoryItem>>().Handle);
            //router.RegisterHandler<DeactivateInventoryItem>(MvcApplication.InventoryKernal.Get<IHandle<DeactivateInventoryItem>>().Handle);
            //router.RegisterHandler<RemoveItemsFromInventory>(MvcApplication.InventoryKernal.Get<IHandle<RemoveItemsFromInventory>>().Handle);
            //router.RegisterHandler<RenameInventoryItem>(MvcApplication.InventoryKernal.Get<IHandle<RenameInventoryItem>>().Handle);

			// Register Event Handlers
            //foreach (var handler in MvcApplication.InventoryKernal.GetAll<IHandle<InventoryItemCreated>>())
            //    router.RegisterHandler<InventoryItemCreated>(handler.Handle);
			
            //foreach (var handler in MvcApplication.InventoryKernal.GetAll<IHandle<InventoryItemRenamed>>())
            //    router.RegisterHandler<InventoryItemRenamed>(handler.Handle);

            //foreach (var handler in MvcApplication.InventoryKernal.GetAll<IHandle<InventoryItemDeactivated>>())
            //    router.RegisterHandler<InventoryItemDeactivated>(handler.Handle);

            //router.RegisterHandler<ItemsCheckedInToInventory>(MvcApplication.InventoryKernal.Get<IHandle<ItemsCheckedInToInventory>>().Handle);
            //router.RegisterHandler<ItemsRemovedFromInventory>(MvcApplication.InventoryKernal.Get<IHandle<ItemsRemovedFromInventory>>().Handle);
        }

        private static object GetCorrectlyInjectedHandler(Type handler)
        {
            return MvcApplication.InventoryKernal.Get(handler);
        }

        private static void RegisterTheCreatedActionWithTheMessageRouter(IRouteMessages router, Type messageType, object action)
        {
            _registerMethod.MakeGenericMethod(messageType).Invoke(router, new[] { action });
        }

        private object CreateTheProperAction(Type commandType, object commandHandler)
        {
            return _createActionMethod.MakeGenericMethod(commandType, commandHandler.GetType()).Invoke(this, new[] { commandHandler });
        }

        public Action<TMessage> CreateAction<TMessage, THandler>(THandler handler)
            where TMessage : class
            where THandler : IHandle<TMessage>
        {
            return message => MvcApplication.InventoryKernal.Get<THandler>().Handle(message);
        }

	    public static void BootStrap()
        {
            new InventoryBootStrapper().BootStrapTheApplication();
        }

		
    }

	
}