using System;
using System.Collections.Generic;
using AgileWorkshop.Cqrs.Core;
using NUnit.Framework;
using System.Linq;

namespace Inventory.Tests
{
	public static class TestExtensions
	{
		public static void WillBeOfType<TType>(this object theEvent)
		{
			Assert.AreEqual(typeof(TType), theEvent.GetType());
		}
		public static void WillBe(this object source, object value)
		{
			Assert.AreEqual(value, source);
		}
		public static void WillNotBe(this object source, object value)
		{
			Assert.AreNotEqual(value, source);
		}
		public static void WillBeSimuliar(this object source, object value)
		{
			Assert.AreEqual(value.ToString(), source.ToString());
		}
		public static void WillNotBeSimuliar(this object source, object value)
		{
			Assert.AreNotEqual(value.ToString(), source.ToString());
		}
		public static void WithMessage(this Exception theException, string message)
		{
			Assert.AreEqual(message, theException.Message);
		}
		public static TEvent Last<TEvent>(this IEnumerable<Event> events)
			where TEvent : Event
        {
            return (TEvent) events.Last();
        }
		public static TDomainEvent As<TDomainEvent>(this object theObject)
		{
			return (TDomainEvent) theObject;
		}
	}
}