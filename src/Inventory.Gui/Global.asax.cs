using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Mvc;
using System.Web.Routing;

using AgileWorkshop.Bus;
using AgileWorkshop.Cqrs.EventStore;
using AgileWorkshop.Cqrs.Reporting;

using Inventory.CommandHandlers;
using Inventory.Commands;
using Inventory.EventHandlers;
using Inventory.Events;

using Ninject;
using Ninject.Modules;

namespace Inventory.Gui
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default",
				// Route name
				"{controller}/{action}/{id}",
				// URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
				);
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);

			// Setup Our new Controller Factory.
			IKernel kernel = new StandardKernel(new InventoryConfigModule());
			ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory(kernel));

			InventoryBootStrapper.BootStrap();

			var router = kernel.Get<IRouteMessages>();

			// TODO: Tidy up into a Helper (Get all IHandles and register with Router

			// Register Command Handlers
			router.RegisterHandler<CheckInItemsToInventory>(kernel.Get<IHandle<CheckInItemsToInventory>>().Handle);
			router.RegisterHandler<CreateInventoryItem>(kernel.Get<IHandle<CreateInventoryItem>>().Handle);
			router.RegisterHandler<DeactivateInventoryItem>(kernel.Get<IHandle<DeactivateInventoryItem>>().Handle);
			router.RegisterHandler<RemoveItemsFromInventory>(kernel.Get<IHandle<RemoveItemsFromInventory>>().Handle);
			router.RegisterHandler<RenameInventoryItem>(kernel.Get<IHandle<RenameInventoryItem>>().Handle);

			// Register Event Handlers
			foreach (var handler in kernel.GetAll<IHandle<InventoryItemCreated>>())
				router.RegisterHandler<InventoryItemCreated>(handler.Handle);
			
			foreach (var handler in kernel.GetAll<IHandle<InventoryItemRenamed>>())
				router.RegisterHandler<InventoryItemRenamed>(handler.Handle);

			foreach (var handler in kernel.GetAll<IHandle<InventoryItemDeactivated>>())
				router.RegisterHandler<InventoryItemDeactivated>(handler.Handle);

			router.RegisterHandler<ItemsCheckedInToInventory>(kernel.Get<IHandle<ItemsCheckedInToInventory>>().Handle);
			router.RegisterHandler<ItemsRemovedFromInventory>(kernel.Get<IHandle<ItemsRemovedFromInventory>>().Handle);

		}
	}

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

			this.Bind<IHandle<CreateInventoryItem>>().To<InventoryCommandHandlers>().InRequestScope();
			this.Bind<IHandle<DeactivateInventoryItem>>().To<InventoryCommandHandlers>().InRequestScope();
			this.Bind<IHandle<RemoveItemsFromInventory>>().To<InventoryCommandHandlers>().InRequestScope();
			this.Bind<IHandle<CheckInItemsToInventory>>().To<InventoryCommandHandlers>().InRequestScope();
			this.Bind<IHandle<RenameInventoryItem>>().To<InventoryCommandHandlers>().InRequestScope();

			this.Bind<ISqlInsertBuilder>().To<SqlInsertBuilder>().InRequestScope();
			this.Bind<ISqlSelectBuilder>().To<SqlSelectBuilder>().InRequestScope();
			this.Bind<ISqlUpdateBuilder>().To<SqlUpdateBuilder>().InRequestScope();
			this.Bind<ISqlDeleteBuilder>().To<SqlDeleteBuilder>().InRequestScope();
			this.Bind<ISqlCreateBuilder>().To<SqlCreateBuilder>().InRequestScope();

			this.Bind<IReportingRepository>().To<SQLiteReportingRepository>().InRequestScope().WithConstructorArgument(
				"sqLiteConnectionString", "Data Source=reportingDataBase.db3");

			this.Bind<IHandle<InventoryItemCreated>>().To<InventoryItemDetailViewHandler>().InRequestScope();
			this.Bind<IHandle<InventoryItemDeactivated>>().To<InventoryItemDetailViewHandler>().InRequestScope();
			this.Bind<IHandle<InventoryItemRenamed>>().To<InventoryItemDetailViewHandler>().InRequestScope();
			this.Bind<IHandle<ItemsRemovedFromInventory>>().To<InventoryItemDetailViewHandler>().InRequestScope();
			this.Bind<IHandle<ItemsCheckedInToInventory>>().To<InventoryItemDetailViewHandler>().InRequestScope();

			this.Bind<IHandle<InventoryItemCreated>>().To<InventoryItemListViewHandler>().InRequestScope();
			this.Bind<IHandle<InventoryItemRenamed>>().To<InventoryItemListViewHandler>().InRequestScope();
			this.Bind<IHandle<InventoryItemDeactivated>>().To<InventoryItemListViewHandler>().InRequestScope();
		}
	}

	public class NinjectControllerFactory : DefaultControllerFactory
	{
		private IKernel kernel = null;

		public NinjectControllerFactory(IKernel kernel)
		{
			this.kernel = kernel;
		}

		protected override IController GetControllerInstance(
			System.Web.Routing.RequestContext requestContext, Type controllerType)
		{
			return controllerType == null ? null : (IController)this.kernel.Get(controllerType);
		}
	}
}