namespace AgileWorkshop.Bus
{
	using System;
	using System.Collections.Generic;
	using System.Threading;

	using AgileWorkshop.Cqrs.Core;

	using Inventory.Infrastructure;

	public class MessageRouter : IRouteMessages
	{
		private readonly Dictionary<Type, List<Action<Message>>> _routes = new Dictionary<Type, List<Action<Message>>>();

		public void Route<T>(T message) where T : Message
		{
			List<Action<Message>> handlers;
			_routes.TryGetValue(message.GetType(), out handlers);

			if (message is Command)
			{
				if (handlers == null)
					throw new InvalidOperationException("no handler registered");

				if (handlers.Count != 1) 
					throw new InvalidOperationException("cannot send to more than one handler");
			}

			foreach(var handler in handlers)
			{
				//dispatch on thread pool for added awesomeness
				var handler1 = handler;
				if (message is Event)
					ThreadPool.QueueUserWorkItem(x => handler1(message));
				else 
					handler1(message);
			}
		}

		public void RegisterHandler<T>(Action<T> handler) where T : Message
		{
			List<Action<Message>> handlers;
			if(!_routes.TryGetValue(typeof(T), out handlers))
			{
				handlers = new List<Action<Message>>();
				_routes.Add(typeof(T), handlers);
			}
			handlers.Add(DelegateAdjuster.CastArgument<Message, T>(x => handler(x)));
		}
	}
}