using Ninject;
using NServiceBus;

namespace AgileWorkshop.Cqrs.NServiceBus
{
	public static class ConfigureAgileWorkshopCqrsNServiceBus
	{
		public static ConfigEventBus AgileWorkshopCqrsNServiceBus(this Configure configure, IKernel kernel)
		{
            return new ConfigEventBus(configure, kernel);
		}
	}
}