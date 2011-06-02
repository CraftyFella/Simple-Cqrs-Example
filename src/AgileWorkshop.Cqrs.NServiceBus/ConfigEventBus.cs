using System;
using System.Collections.Generic;

using AgileWorkshop.Bus;

using Inventory.Events;

using NServiceBus;
using NServiceBus.Unicast;

namespace AgileWorkshop.Cqrs.NServiceBus
{
	public class ConfigEventBus : Configure
	{
		private readonly Configure configure;

		public ConfigEventBus(Configure configure)
		{
			this.configure = configure;
		}

		public ConfigEventBus SubscribeForDomainEvents()
		{
			// TODO: Get Event Handlers some how!!!! rather than hard coding
			//var eventHandlerTypes = new[] { typeof(InventoryItemListViewHandler), typeof(InventoryItemDetailViewHandler) };

			// TODO: Get IRouteMessages from IoC
			var eventBus = new FakeBus(new MessageRouter());

			// Override the IEventBus in IoC with the NsB one
			configure.Configurer.RegisterSingleton<IEventBus>(eventBus);

			//var configEventBus = new ConfigEventBus();
			//configEventBus.Configure(this);

			this.Configure(configure);
			
			return this;
		}


		public void Configure(Configure configure1)
		{
			var domainEventBusConfig = GetConfigSection<DomainEventBusConfig>();

			//TODO: Replace this with a Find classes that inherit from Event In Assembly blar blar blar
			var domainEventTypes = new[]
        		{
        			typeof(InventoryItemCreated), typeof(InventoryItemDeactivated), typeof(InventoryItemRenamed),
        			typeof(ItemsCheckedInToInventory), typeof(ItemsRemovedFromInventory)
        		};

			var domainEventsTypesWrappedWithNServiceBusType = new List<Type>();

			var bus = (UnicastBus)configure1
				.MsmqTransport()
				.UnicastBus()
					.LoadMessageHandlers(new First<NServiceBusEventMessageHandler>())
					.CreateBus();

			RegisterAssemblyDomainEventSubscriptionMappings(domainEventBusConfig, domainEventTypes, domainEventsTypesWrappedWithNServiceBusType, bus);

			// TODO: Cast to UnicastBus isn't working
			bus.Started += (s, e) => domainEventsTypesWrappedWithNServiceBusType.ForEach(bus.Subscribe);
		}

		private static void RegisterAssemblyDomainEventSubscriptionMappings(DomainEventBusConfig domainEventBusConfig, IEnumerable<Type> domainEventTypes, ICollection<Type> domainEventMessageTypes, UnicastBus bus)
		{
			var domainEventMessageType = typeof(NServiceBusEventMessage<>);
			foreach (DomainEventEndpointMapping mapping in domainEventBusConfig.DomainEventEndpointMappings)
			{
				foreach (var domainEventType in domainEventTypes)
				{
					if (DomainEventsIsAssembly(domainEventType, mapping.DomainEvents))
					{
						var messageType = domainEventMessageType.MakeGenericType(domainEventType);
						domainEventMessageTypes.Add(messageType);
						bus.RegisterMessageType(messageType, mapping.Endpoint, false);
					}
				}
			}
		}

		private static bool DomainEventsIsDomainEvent(Type domainEventType, string domainEvents)
		{
			return domainEventType.FullName.ToLower() == domainEvents.ToLower()
				   || domainEventType.AssemblyQualifiedName.ToLower() == domainEvents.ToLower();
		}

		private static bool DomainEventsIsAssembly(Type domainEventType, string domainEvents)
		{
			return domainEventType.Assembly.GetName().Name.ToLower() == domainEvents.ToLower();
		}
	}
}
