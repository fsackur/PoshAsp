using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace PoshAsp.Models
{
    public class LogEntry
    {
        private ObjectId _Id;
        private DateTime _Timestamp;
        private string _Username;
        private string _Text;
        private Dictionary<string, object> _Data = new Dictionary<string,object>();

        public LogEntry()
        {
            _Timestamp = DateTime.Now;
        }

        public LogEntry(string LogName, string EntryId)
        {
            MongoClient client = new MongoClient(Properties.Settings.Default.MongoConnectionString);
            MongoServer server = client.GetServer();
            MongoDatabase db = server.GetDatabase(Properties.Settings.Default.MongoDb);
            MongoCollection<LogEntry> entries = db.GetCollection<LogEntry>(LogName);
            LogEntry entry = entries.FindOne(Query<LogEntry>.EQ(e => e.Id, ObjectId.Parse(EntryId)));

            _Id = entry.Id;
            _Timestamp = entry.Timestamp;
            _Username = entry.Username;
            _Text = entry.Text;
        }

        public void Write(string LogName)
        {
            MongoClient client = new MongoClient(Properties.Settings.Default.MongoConnectionString);
            MongoServer server = client.GetServer();
            MongoDatabase db = server.GetDatabase(Properties.Settings.Default.MongoDb);
            MongoCollection entries = db.GetCollection<LogEntry>(LogName);
            entries.Insert(this);
        }

        public ObjectId Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public DateTime Timestamp
        {
            get { return _Timestamp; }
            set { _Timestamp = value; }
        }

        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }

        public Dictionary<string, object> Data
        {
            get { return _Data; }
            set { _Data = value; }
        }
    }
}