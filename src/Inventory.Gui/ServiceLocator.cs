using AgileWorkshop.Bus;
using AgileWorkshop.Cqrs.Core;
using Inventory.Reporting;

namespace Inventory.Gui
{
	using AgileWorkshop.Cqrs.Reporting;

	public static class ServiceLocator
    {
		public static IEventBus EventBus { get; set; }
        public static ICommandBus CommandBus { get; set; }
    	public static IReportingRepository ReportingRepository { get; set; }
    }
}