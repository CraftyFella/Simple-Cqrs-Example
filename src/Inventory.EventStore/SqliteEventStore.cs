﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.Serialization;
using AgileWorkshop.Bus;
using AgileWorkshop.Cqrs.Core;

namespace Inventory.EventStore
{
	public class SqliteEventStore : IEventStore
	{
		private readonly IEventPublisher _publisher;
		private readonly IFormatter formatter;
	    private SQLiteConnection _sqLiteConnectionString;

	    public SqliteEventStore(IEventPublisher publisher, IFormatter formatter, string sqLiteConnectionString)
		{
			_publisher = publisher;
	        _sqLiteConnectionString = new SQLiteConnection(sqLiteConnectionString);
	        this.formatter = formatter;
		}

		// Replaced with Sqlite db
		//private readonly Dictionary<Guid, List<EventDescriptor>> _current = new Dictionary<Guid, List<EventDescriptor>>(); 
        
		public void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
		{
			List<EventDescriptor> eventDescriptors = GetEventsByAggregateId(aggregateId);
			
			if(eventDescriptors.Any() && eventDescriptors.Last().Version != expectedVersion && expectedVersion != -1)
			{
				throw new ConcurrencyException();
			}

			var i = expectedVersion;
            using (var sqliteConnection = new SQLiteConnection(_sqLiteConnectionString))
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
                            _publisher.Publish(@event);
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

        private static void SaveEvent(EventDescriptor eventDescriptor, SQLiteTransaction transaction)
        {
            const string commandText = "INSERT INTO Events VALUES(@aggregateId, @eventData, @version)";
            using (var sqLiteCommand = new SQLiteCommand(commandText, transaction.Connection, transaction))
            {
                sqLiteCommand.Parameters.Add(new SQLiteParameter("@aggregateId", eventDescriptor.AggregateId));
                sqLiteCommand.Parameters.Add(new SQLiteParameter("@eventData", eventDescriptor.EventData));
                sqLiteCommand.Parameters.Add(new SQLiteParameter("@version", eventDescriptor.Version));

                sqLiteCommand.ExecuteNonQuery();
            }
        }

		private List<EventDescriptor> GetEventsByAggregateId(Guid aggregateId)
		{
			var eventDescriptors = new List<EventDescriptor>();

            const string commandText = @"SELECT eventData, version FROM Events WHERE aggregateId = @aggregateId ORDER BY Version ASC;";

            using (var sqliteConnection = new SQLiteConnection(_sqLiteConnectionString))
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
                                    var eventDescriptor = new EventDescriptor
                                                              {
                                                                  AggregateId = aggregateId,
                                                                  EventData = (byte[]) sqLiteDataReader["eventData"],
                                                                  Version = (int) sqLiteDataReader["version"]
                                                              };

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

		
	}
}