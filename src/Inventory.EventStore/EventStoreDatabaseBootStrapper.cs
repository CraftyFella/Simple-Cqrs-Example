using System.Data.Common;
using System.Data.SQLite;
using System.IO;

namespace Inventory.EventStore
{
    public class EventStoreDatabaseBootStrapper
    {
        public const string dataBaseFile = "eventStore.db3";

        public static void BootStrap()
        {
            new EventStoreDatabaseBootStrapper().CreateDatabaseSchemaIfNeeded();
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

        private static void DoCreateDatabaseSchema()
        {
            SQLiteConnection.CreateFile(dataBaseFile);

            var sqLiteConnection = new SQLiteConnection(string.Format("Data Source={0}", dataBaseFile));

            sqLiteConnection.Open();

            using (DbTransaction dbTrans = sqLiteConnection.BeginTransaction())
            {
                using (DbCommand sqLiteCommand = sqLiteConnection.CreateCommand())
                {
                    const string eventsTables = @"
                        CREATE TABLE Events
                        (
                            [AggregateId] [uniqueidentifier] not null,
                            [EventData] [binary] not null,
                            [Version] [int] not null
                        );
                        ";
                    sqLiteCommand.CommandText = eventsTables;
                    sqLiteCommand.ExecuteNonQuery();

                    
                }
                dbTrans.Commit();
            }

            sqLiteConnection.Close();
        }
    }
}