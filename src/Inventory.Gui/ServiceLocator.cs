using AgileWorkshop.Cqrs.Core;
using Inventory.Reporting;

namespace Inventory.Gui
{
    public static class ServiceLocator
    {
        public static FakeBus Bus { get; set; }
    	public static IReportingRepository ReportingRepository { get; set; }
    }
}