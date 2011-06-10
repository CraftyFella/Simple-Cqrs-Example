using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

using AgileWorkshop.Bus;
using AgileWorkshop.Cqrs.Core;
using Inventory.Commands;
using Inventory.Events;
using Inventory.Infrastructure;
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

		internal static readonly IKernel InventoryKernal = new StandardKernel(new InventoryConfigModule());

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);

		    
			// Setup Our new Controller Factory.
			//IKernel kernel = new StandardKernel(new InventoryConfigModule());
			ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory(InventoryKernal));

			InventoryBootStrapper.BootStrap();

			

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