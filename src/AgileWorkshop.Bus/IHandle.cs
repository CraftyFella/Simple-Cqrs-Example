namespace AgileWorkshop.Cqrs.Core
{
	public interface IHandle<T>
	{
		void Handle(T message);
	}
}