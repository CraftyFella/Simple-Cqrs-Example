using AgileWorkshop.Cqrs.Core;

namespace AgileWorkshop.Bus
{
	public interface ICommandBus
	{
		void Send<T>(T command) where T : Command;

	}
}