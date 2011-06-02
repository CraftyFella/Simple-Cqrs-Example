
using AgileWorkshop.Cqrs.NServiceBus;

using NServiceBus;

namespace Inventory.EventHandlers
{
	public class EndpointConfiguration : IConfigureThisEndpoint, AsA_Server
	{
	}

	public class AnotherClass : IWantCustomInitialization
	{
		public void Init()
		{
			Configure.With()
				.DefaultBuilder()
				.BinarySerializer()
				//.SimpleCqrs(new SimpleCqrsRuntime<UnityServiceLocator>())
				.AgileWorkshopCqrsNServiceBus()
				.SubscribeForDomainEvents();    // Tells SimpleCqrs to subscribe for domain events

			// The DomainEventBusConfig element in the Web.config tell SimpleCqrs which domain events to listen for and the queue 
			// to listen for them from.
		}
	}
}