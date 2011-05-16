using AgileWorkshop.Cqrs.Core;

namespace AgileWorkshop.Bus
{
    public class FakeBus : ICommandBus, IEventBus
    {

		private object lockObject = new object();

		public FakeBus(IRouteMessages messageRouter)
    	{
    		this.messageRouter = messageRouter;
    	}

    	private readonly IRouteMessages messageRouter;

        public void Send<T>(T command) where T : Command
        {
			lock(lockObject)
			{
				this.messageRouter.Route(command);
			}
        }

        public void Publish<T>(T @event) where T : Event
        {
			lock(lockObject)
			{
				this.messageRouter.Route(@event);
			}
        }
    }
}
