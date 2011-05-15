using AgileWorkshop.Cqrs.Core;

namespace AgileWorkshop.Bus
{
	public interface IEventPublisher
	{
		void Publish<T>(T @event) where T : Event;
	}
}