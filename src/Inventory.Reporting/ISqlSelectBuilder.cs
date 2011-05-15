using System.Collections.Generic;

namespace Inventory.Reporting
{
	public interface ISqlSelectBuilder 
	{
		string CreateSqlSelectStatementFromDto<TDto>() ;
		string CreateSqlSelectStatementFromDto<TDto>(IEnumerable<KeyValuePair<string, object>> example) where TDto : class;
	}
}