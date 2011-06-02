
using NServiceBus;

namespace Inventory.NServiceBus.EventPublisherTest
{
	public class EndpointConfiguration : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
	{
		public void Init()
		{
			Configure.With().DefaultBuilder().BinarySerializer().InMemorySubscriptionStorage();
		}
	}
}