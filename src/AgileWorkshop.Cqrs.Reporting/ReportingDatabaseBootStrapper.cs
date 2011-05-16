﻿namespace AgileWorkshop.Cqrs.Reporting
{
	using System;
	using System.Collections.Generic;
	using System.Data.Common;
	using System.Data.SQLite;
	using System.IO;

	using Inventory.Reporting.Dto;

	public class ReportingDatabaseBootStrapper
	{
		public const string dataBaseFile = "reportingDataBase.db3";
		private readonly List<Type> _dtos = new List<Type>
		                                    {
		                                    	typeof(InventoryItemListDto), 
		                                    	typeof(InventoryItemDetailsDto), 
		                                    };
		private readonly SqlCreateBuilder _sqlCreateBuilder = new SqlCreateBuilder();

		public static void BootStrap()
		{
			new ReportingDatabaseBootStrapper().CreateDatabaseSchemaIfNeeded();
		}

		public void ReCreateDatabaseSchema()
		{
			if (File.Exists(dataBaseFile))
				File.Delete(dataBaseFile);

			DoCreateDatabaseSchema();            
		}

		public void CreateDatabaseSchemaIfNeeded()
		{
			if (File.Exists(dataBaseFile))
				return;

			DoCreateDatabaseSchema();
		}

		private void DoCreateDatabaseSchema()
		{
			SQLiteConnection.CreateFile(dataBaseFile);

			var sqLiteConnection = new SQLiteConnection(string.Format("Data Source={0}", dataBaseFile));

			sqLiteConnection.Open();

			using (DbTransaction dbTrans = sqLiteConnection.BeginTransaction())
			{
				using (DbCommand sqLiteCommand = sqLiteConnection.CreateCommand())
				{
					foreach (var dto in _dtos)
					{
						sqLiteCommand.CommandText = _sqlCreateBuilder.CreateSqlCreateStatementFromDto(dto);
						sqLiteCommand.ExecuteNonQuery();
					}
				}
				dbTrans.Commit();
			}

			sqLiteConnection.Close();
		}
	}
}