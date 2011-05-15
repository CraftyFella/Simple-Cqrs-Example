using AgileWorkshop.Cqrs.Core;

namespace AgileWorkshop.Bus
{
	public interface ICommandSender
	{
		void Send<T>(T command) where T : Command;

	}
}