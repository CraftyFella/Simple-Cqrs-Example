using System;

namespace Inventory.Reporting
{
	public interface ISqlCreateBuilder
	{
		string CreateSqlCreateStatementFromDto(Type dtoType);
	}
}