namespace AgileWorkshop.Cqrs.Reporting
{
	using System.Collections.Generic;

	public interface ISqlSelectBuilder 
	{
		string CreateSqlSelectStatementFromDto<TDto>() ;
		string CreateSqlSelectStatementFromDto<TDto>(IEnumerable<KeyValuePair<string, object>> example) where TDto : class;
	}
}