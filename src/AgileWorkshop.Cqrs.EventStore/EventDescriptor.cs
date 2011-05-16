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

		public Event Event
		{ 
			get { return Deserialize<Event>(EventData); }
		}

		public byte[] EventData { get; set; }

		public int Id { get; set; }
		public Guid AggregateId { get; set; }
		public int Version { get; set; }
            
		public EventDescriptor()
		{
			
		}

		public EventDescriptor(Guid aggregateId, Event @event, int version)
		{
			EventData = Serialize(@event);
			Version = version;
			AggregateId = aggregateId;
		}

		private byte[] Serialize(object theObject)
		{
			using (var memoryStream = new MemoryStream())
			{
				formatter.Serialize(memoryStream, theObject);
				return memoryStream.ToArray();
			}
		}

		private TType Deserialize<TType>(byte[] bytes)
		{
			using (var memoryStream = new MemoryStream(bytes))
			{
				return (TType)formatter.Deserialize(memoryStream);
			}
		}
	}
}