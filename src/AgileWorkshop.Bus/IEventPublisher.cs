namespace AgileWorkshop.Cqrs.Core
{
	public interface IEventPublisher
	{
		void Publish<T>(T @event) where T : Event;
	}
}