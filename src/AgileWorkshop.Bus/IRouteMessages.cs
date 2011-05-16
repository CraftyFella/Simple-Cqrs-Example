using System;
using AgileWorkshop.Cqrs.Core;

namespace AgileWorkshop.Bus
{

	public interface IRouteMessages
	{
		void Route<T>(T message) where T : Message;
		void RegisterHandler<T>(Action<T> handler) where T : Message;
	}
}