namespace AgileWorkshop.Cqrs.Reporting
{
	using System;

	public interface ISqlCreateBuilder
	{
		string CreateSqlCreateStatementFromDto(Type dtoType);
	}
}