﻿namespace AgileWorkshop.Cqrs.EventStore
{
	using System;

	using AgileWorkshop.Cqrs.Core;

	public class DomainRepository<T> : IDomainRepository<T> where T: AggregateRoot, new() //shortcut you can do as you see fit with new()
	{
		private readonly IEventStore _storage;

		public DomainRepository(IEventStore storage)
		{
			_storage = storage;
		}

		public void Save(T aggregate, int expectedVersion)
		{
			_storage.SaveEvents(aggregate.Id, aggregate.GetUncommittedChanges(), expectedVersion);
		}

		public T GetById(Guid id)
		{
			var obj = new T();//lots of ways to do this
			var e = _storage.GetEventsForAggregate(id);
			obj.LoadsFromHistory(e);
			return obj;
		}
	}
}