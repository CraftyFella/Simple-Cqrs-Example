namespace AgileWorkshop.Cqrs.EventStore
{
	using System;
	using System.Collections.Generic;

	using AgileWorkshop.Cqrs.Core;

	public interface IEventStore
	{
		void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion);
		List<Event> GetEventsForAggregate(Guid aggregateId);
	}
}