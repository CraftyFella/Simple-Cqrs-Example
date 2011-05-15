﻿using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Reflection;

namespace Inventory.Reporting
{
	public class SqlCreateBuilder : ISqlCreateBuilder
	{
		private readonly IDictionary<Type, string> _columnTypes;

		public SqlCreateBuilder()
		{
			_columnTypes = new Dictionary<Type, string>
			               {
			               	{typeof (bool), "bool"},
			               	{typeof (Int32), "int"},
			               	{typeof (string), "nvarchar(250)"},
			               	{typeof (double), "numeric"},
			               	{typeof (decimal), "numeric"},
			               	{typeof (float), "numeric"},
			               	{typeof (Guid), "uniqueidentifier"},
			               };
		}

		public string CreateSqlCreateStatementFromDto(Type dtoType)
		{
			var tableName = dtoType.Name;

			return string.Format("CREATE TABLE {0} ({1});", tableName, GetColumns(dtoType));
		}

		private string GetColumns(Type dtoType)
		{
			var properties = dtoType.GetPropertiesAndFields();

			return string.Join(",", properties
			                        	.Where(Where)
			                        	.Select(x => GetColumn(x)).ToArray());
		}

		private static bool Where(IProperty propertyInfo)
		{
			return !propertyInfo.PropertyType.IsGenericType;
		}

		private string GetColumn(IProperty propertyInfo)
		{
			return propertyInfo.Name == "Id"
			       	? string.Format("[{0}] [{1}] primary key", propertyInfo.Name, GetColumnType(propertyInfo))
			       	: propertyInfo.Name.EndsWith("Id")
			       	  	? string.Format("[{0}] [{1}] foreing key", propertyInfo.Name, GetColumnType(propertyInfo))
			       	  	: string.Format("[{0}] [{1}]", propertyInfo.Name, GetColumnType(propertyInfo));
		}

		private string GetColumnType(IProperty propertyInfo)
		{
			var type = propertyInfo.PropertyType;
			if (!_columnTypes.ContainsKey(type))
				throw new Exception(string.Format("The key {0} was not found!", type.Name));

			return _columnTypes[type];
		}
	}
}