using System;
using System.Collections.Generic;
using System.Linq;
using AgileWorkshop.Bus;
using AgileWorkshop.Cqrs.Core;

using Moq;
using NUnit.Framework;

namespace Inventory.Tests
{
	using AgileWorkshop.Cqrs.EventStore;

	[Specification]
	public abstract class CommandTestFixture<TCommand, TCommandHandler, TAggregateRoot> 
		where TCommand : Command
		where TCommandHandler : class, IHandle<TCommand>
		where TAggregateRoot : AggregateRoot, new()
	{
		private IDictionary<Type, object> mocks;

		protected TAggregateRoot AggregateRoot;
		protected IHandle<TCommand> CommandHandler;
		protected Exception CaughtException;
		protected IEnumerable<Event> PublishedEvents;
		protected virtual void SetupDependencies() { }
		protected virtual IEnumerable<Event> Given() 
		{
			return new List<Event>();
		}
		protected virtual void Finally() { }
		protected abstract TCommand When();

		[Given]
		public void Setup()
		{
			mocks = new Dictionary<Type, object>();
			CaughtException = new ThereWasNoExceptionButOneWasExpectedException();
			AggregateRoot = new TAggregateRoot();
			AggregateRoot.LoadsFromHistory(Given());

			CommandHandler = BuildCommandHandler();

			SetupDependencies();
			try
			{
				CommandHandler.Handle(When());
				PublishedEvents = AggregateRoot.GetUncommittedChanges();
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
			return (Mock<TType>)mocks[typeof(TType)];
		}

		private IHandle<TCommand> BuildCommandHandler()
		{
			var constructorInfo = typeof(TCommandHandler).GetConstructors().First();

			foreach (var parameter in constructorInfo.GetParameters())
			{
				if (parameter.ParameterType == typeof(IDomainRepository<TAggregateRoot>))
				{
					var repositoryMock = new Mock<IDomainRepository<TAggregateRoot>>();
					repositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(AggregateRoot);
					repositoryMock.Setup(x => x.Save(It.IsAny<TAggregateRoot>(), It.IsAny<int>())).Callback<TAggregateRoot, int>((r, i) => AggregateRoot = r);
					mocks.Add(parameter.ParameterType, repositoryMock);
					continue;
				}

				mocks.Add(parameter.ParameterType, CreateMock(parameter.ParameterType));
			}

			return (IHandle<TCommand>)constructorInfo.Invoke(mocks.Values.Select(x => ((Mock) x).Object).ToArray());
		}

		private static object CreateMock(Type type)
		{
			var constructorInfo = typeof (Mock<>).MakeGenericType(type).GetConstructors().First();
			return constructorInfo.Invoke(new object[]{});
		}
	}

	public class ThereWasNoExceptionButOneWasExpectedException : Exception {}
	public class GivenAttribute : SetUpAttribute { }

    public class ThenAttribute : TestAttribute { }
	public class SpecificationAttribute : TestFixtureAttribute
	{
	}
}