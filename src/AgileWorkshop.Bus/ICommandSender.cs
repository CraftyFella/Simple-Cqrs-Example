﻿namespace AgileWorkshop.Cqrs.Core
{
	public interface ICommandSender
	{
		void Send<T>(T command) where T : Command;

	}
}