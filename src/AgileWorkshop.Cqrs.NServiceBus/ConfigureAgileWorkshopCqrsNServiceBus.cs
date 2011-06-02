using NServiceBus;

namespace AgileWorkshop.Cqrs.NServiceBus
{
	public static class ConfigureAgileWorkshopCqrsNServiceBus
	{
		public static ConfigEventBus AgileWorkshopCqrsNServiceBus(this Configure configure)
		{
			return new ConfigEventBus(configure);
		}
	}
}