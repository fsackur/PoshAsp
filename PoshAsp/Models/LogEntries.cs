using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace PoshAsp.Models
{
    public class LogEntries : List<LogEntry>
    {
        public LogEntries()
        {

        }

        public LogEntries(string LogName)
        {
            MongoClient client = new MongoClient(Properties.Settings.Default.MongoConnectionString);
            MongoServer server = client.GetServer();
            MongoDatabase db = server.GetDatabase(Properties.Settings.Default.MongoDb);
            MongoCollection<BsonDocument> entries = db.GetCollection<BsonDocument>(LogName);

            foreach (LogEntry entry in entries.FindAllAs<LogEntry>())
            {
                this.Add(entry);
            }
        }
    }
}