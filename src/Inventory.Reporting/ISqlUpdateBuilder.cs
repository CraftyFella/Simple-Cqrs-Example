﻿namespace Inventory.Reporting
{
	public interface ISqlUpdateBuilder
	{
		string GetUpdateString<TDto>(object update, object where) where TDto : class;
	}
}