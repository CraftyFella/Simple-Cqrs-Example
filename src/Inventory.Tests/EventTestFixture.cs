using System;
using System.Collections.Generic;
using System.Linq;
using AgileWorkshop.Bus;
using AgileWorkshop.Cqrs.Core;
using Inventory.Reporting;
using Moq;

namespace Inventory.Tests.Scenarios.Adding_a_new_inventory_item
{
	using AgileWorkshop.Cqrs.Reporting;

	[Specification]
    public abstract class EventTestFixture<TEvent, TEventHandler>
        where TEvent : class
        where TEventHandler : class, IHandle<TEvent>
    {
        private IDictionary<Type, object> mocks;

        protected Exception CaughtException;
        protected IHandle<TEvent> EventHandler;
        protected virtual void SetupDependencies() { }
        protected abstract TEvent When();
        protected virtual void Finally() { }

        [Given]
        public void Setup()
        {
            mocks = new Dictionary<Type, object>();
            CaughtException = new ThereWasNoExceptionButOneWasExpectedException();
            EventHandler = BuildCommandHandler();
            SetupDependencies();

            try
            {
                EventHandler.Handle(When());
            }
            catch (Exception exception)
            {
                CaughtException = exception;
            }
            finally
            {
                Finally();
            }
        }

        public Mock<TType> OnDependency<TType>() where TType : class
        {
            if (!mocks.ContainsKey(typeof(TType)))
                throw new Exception(string.Format("The event handler '{0}' does not have a dependency upon '{1}'", typeof(TEventHandler).FullName, typeof(TType).FullName));

            return (Mock<TType>)mocks[typeof(TType)];
        }

        private IHandle<TEvent> BuildCommandHandler()
        {
            var constructorInfo = typeof(TEventHandler).GetConstructors().First();

            foreach (var parameter in constructorInfo.GetParameters())
            {
                if (parameter.ParameterType == typeof(IReportingRepository))
                {
                    var repositoryMock = new Mock<IReportingRepository>();
                    mocks.Add(parameter.ParameterType, repositoryMock);
                    continue;
                }

                mocks.Add(parameter.ParameterType, CreateMock(parameter.ParameterType));
            }

            return (IHandle<TEvent>)constructorInfo.Invoke(mocks.Values.Select(x => ((Mock)x).Object).ToArray());
        }

        private static object CreateMock(Type type)
        {
            var constructorInfo = typeof(Mock<>).MakeGenericType(type).GetConstructors().First();
            return constructorInfo.Invoke(new object[] { });
        }
    }
}