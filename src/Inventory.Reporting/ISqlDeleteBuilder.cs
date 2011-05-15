using System.Collections.Generic;

namespace Inventory.Reporting
{
	public interface ISqlDeleteBuilder
	{
		string CreateSqlDeleteStatementFromDto<TDto>();
		string CreateSqlDeleteStatementFromDto<TDto>(IEnumerable<KeyValuePair<string, object>> example) where TDto : class;
	}
}