using AgileWorkshop.Cqrs.Core;

namespace AgileWorkshop.Bus
{
	public interface IEventBus
	{
		void Publish<T>(T @event) where T : Event;
	}
}