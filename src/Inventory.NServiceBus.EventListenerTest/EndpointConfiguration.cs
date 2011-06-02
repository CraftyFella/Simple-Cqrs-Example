using System;

using AgileWorkshop.Bus;
using AgileWorkshop.Cqrs.Core;

using NServiceBus;
using NServiceBus.Hosting.Profiles;
using NServiceBus.ObjectBuilder;

namespace Inventory.NServiceBus.EventListenerTest
{
	public class EndpointConfiguration : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
	{
		public void Init()
		{
		    Configure.With().DefaultBuilder().BinarySerializer();
		}
	}

	public class Behaviour : IHandleProfile<Lite>
	{
		public void ProfileActivated()
		{
			Configure.Instance.Configurer.ConfigureComponent<DoesNothingEventBus>(ComponentCallModelEnum.Singleton);
	

		}
	}

	public class DoesNothingEventBus : IEventBus
	{
		public void Publish<T>(T @event) where T : Event
		{
			return;
		}
	}
}