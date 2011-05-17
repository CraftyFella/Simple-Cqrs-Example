namespace AgileWorkshop.Cqrs.EventStore
{
	using System;
	using System.IO;
	using System.Runtime.Serialization;
	using System.Runtime.Serialization.Formatters.Binary;

	using AgileWorkshop.Cqrs.Core;

	public class EventDescriptor
	{
		private readonly IFormatter formatter = new BinaryFormatter();

		public Event Event { get; private set; }
		public Guid AggregateId { get; private set; }
		public int Version { get; private set; }
        
		public EventDescriptor(Guid aggregateId, Event @event, int version)
		{
			Event = @event;
			Version = version;
			AggregateId = aggregateId;
		}

	}
}