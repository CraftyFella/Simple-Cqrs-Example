using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

using AgileWorkshop.Bus;
using AgileWorkshop.Cqrs.Core;

namespace AgileWorkshop.Cqrs.EventStore
{
	

	public class SqliteEventStore : IEventStore
	{
		private readonly IEventBus bus;
		private readonly IFormatter formatter;
	    private readonly SQLiteConnection sqLiteConnectionString;

	    public SqliteEventStore(IEventBus bus, IFormatter formatter, string sqLiteConnectionString)
		{
			this.bus = bus;
	        this.sqLiteConnectionString = new SQLiteConnection(sqLiteConnectionString);
	        this.formatter = formatter;
		}
        
		public void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
		{
			List<EventDescriptor> eventDescriptors = GetEventsByAggregateId(aggregateId);
			
			if(eventDescriptors.Any() && eventDescriptors.Last().Version != expectedVersion && expectedVersion != -1)
			{
				throw new ConcurrencyException();
			}

			var i = expectedVersion;
            using (var sqliteConnection = new SQLiteConnection(sqLiteConnectionString))
			{
                sqliteConnection.Open();

                using (var sqliteTransaction = sqliteConnection.BeginTransaction())
				{
                    try
                    {
                        foreach (var @event in events)
                        {
                            i++;
                            @event.Version = i;
                            SaveEvent(new EventDescriptor(aggregateId, @event, i), sqliteTransaction);
                            this.bus.Publish(@event);
                        }
                        sqliteTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        sqliteTransaction.Rollback();
                        throw;
                    }
				}
			}
		}

        private void SaveEvent(EventDescriptor eventDescriptor, SQLiteTransaction transaction)
        {
            const string commandText = "INSERT INTO Events VALUES(@aggregateId, @eventData, @version)";
            using (var sqLiteCommand = new SQLiteCommand(commandText, transaction.Connection, transaction))
            {
                sqLiteCommand.Parameters.Add(new SQLiteParameter("@aggregateId", eventDescriptor.AggregateId));
                sqLiteCommand.Parameters.Add(new SQLiteParameter("@eventData", Serialize(eventDescriptor.Event)));
                sqLiteCommand.Parameters.Add(new SQLiteParameter("@version", eventDescriptor.Version));

                sqLiteCommand.ExecuteNonQuery();
            }
        }

		private List<EventDescriptor> GetEventsByAggregateId(Guid aggregateId)
		{
			var eventDescriptors = new List<EventDescriptor>();

            const string commandText = @"SELECT eventData, version FROM Events WHERE aggregateId = @aggregateId ORDER BY Version ASC;";

            using (var sqliteConnection = new SQLiteConnection(sqLiteConnectionString))
            {
                sqliteConnection.Open();

                using (var sqliteTransaction = sqliteConnection.BeginTransaction())
                {
                    try
                    {
                        using (var sqliteCommand = new SQLiteCommand(commandText, sqliteTransaction.Connection, sqliteTransaction))
                        {
                            sqliteCommand.Parameters.Add(new SQLiteParameter("@aggregateId", aggregateId));
                            using (var sqLiteDataReader = sqliteCommand.ExecuteReader())
                            {
                                while (sqLiteDataReader.Read())
                                {
                                	var eventDescriptor = new EventDescriptor(
                                		aggregateId,
                                		this.Deserialize<Event>((byte[])sqLiteDataReader["eventData"]),
                                		(int)sqLiteDataReader["version"]);
                                    eventDescriptors.Add(eventDescriptor);
                                }
                            }
                        }
                        sqliteTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        sqliteTransaction.Rollback();
                        throw;
                    }
                }
            }
            return eventDescriptors;
		}

		public  List<Event> GetEventsForAggregate(Guid aggregateId)
		{
			List<EventDescriptor> eventDescriptors = GetEventsByAggregateId(aggregateId);
			if (eventDescriptors.Count == 0)
			{
				throw new AggregateNotFoundException();
			}
			   
			return eventDescriptors.Select(desc => desc.Event).ToList();
		}

		private byte[] Serialize(object theObject)
		{
			using (var memoryStream = new MemoryStream())
			{
				formatter.Serialize(memoryStream, theObject);
				return memoryStream.ToArray();
			}
		}

		private TType Deserialize<TType>(byte[] bytes)
		{
			using (var memoryStream = new MemoryStream(bytes))
			{
				return (TType)formatter.Deserialize(memoryStream);
			}
		}
	}
}