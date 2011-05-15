using System;
using AgileWorkshop.Cqrs.Core;

namespace Inventory.EventStore
{
	public interface IDomainRepository<T> where T : AggregateRoot, new()
	{
		void Save(T aggregate, int expectedVersion);
		T GetById(Guid id);
	}
}