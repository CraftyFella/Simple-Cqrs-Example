namespace AgileWorkshop.Bus
{
	public interface IHandle<T>
	{
		void Handle(T message);
	}
}