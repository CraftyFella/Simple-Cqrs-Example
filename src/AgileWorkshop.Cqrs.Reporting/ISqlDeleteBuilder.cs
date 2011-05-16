namespace AgileWorkshop.Cqrs.Reporting
{
	using System.Collections.Generic;

	public interface ISqlDeleteBuilder
	{
		string CreateSqlDeleteStatementFromDto<TDto>();
		string CreateSqlDeleteStatementFromDto<TDto>(IEnumerable<KeyValuePair<string, object>> example) where TDto : class;
	}
}