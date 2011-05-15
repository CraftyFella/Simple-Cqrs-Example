namespace Inventory.Reporting
{
	public interface ISqlInsertBuilder
	{
		string CreateSqlInsertStatementFromDto<TDto>() where TDto : class;
	}
}