using System;

namespace AgileWorkshop.Cqrs.Core
{
	[Serializable]
	public class Event : Message
	{
        public int Version;
	}
}

