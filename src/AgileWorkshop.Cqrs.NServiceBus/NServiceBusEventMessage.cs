using System;

using AgileWorkshop.Cqrs.Core;

namespace AgileWorkshop.Cqrs.NServiceBus
{
	[Serializable]
	public class NServiceBusEventMessage<TEvent> : INServiceBusEventMessage
		where TEvent : Event
	{
		public TEvent RealEvent { get; set; }

		public string Header { get; set; }

		Event INServiceBusEventMessage.RealEvent
		{
			get { return RealEvent; }
			set { RealEvent = (TEvent)value; }
		}
	}
}