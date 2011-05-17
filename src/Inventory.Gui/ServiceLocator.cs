using AgileWorkshop.Bus;


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