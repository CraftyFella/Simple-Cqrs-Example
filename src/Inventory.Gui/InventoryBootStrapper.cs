using AgileWorkshop.Bus;
using AgileWorkshop.Cqrs.Configuration;
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
            BusBootStrapper.BootStrap(MvcApplication.InventoryKernal);
        }

	    public static void BootStrap()
        {
            new InventoryBootStrapper().BootStrapTheApplication();
        }

		
    }

	
}