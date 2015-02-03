using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace PoshAsp.Models
{
    public class AuthToken
    {
        private ObjectId _Id;
        private string _Username;
        private DateTime _ValidUntil;

        public AuthToken(string Username, string Password)
        {
            MongoClient client = new MongoClient(Properties.Settings.Default.MongoConnectionString);
            MongoServer server = client.GetServer();
            MongoDatabase db = server.GetDatabase(Properties.Settings.Default.MongoDb);
            MongoCollection<BsonDocument> users = db.GetCollection<BsonDocument>("users");
            BsonDocument user = users.FindOne(Query.EQ("username", Username));

            if (user["password"].AsString == Password)
            {
                _Username = Username;
                _ValidUntil = DateTime.Now.AddHours(1);

                MongoCollection<AuthToken> tokens = db.GetCollection<AuthToken>("tokens");
                tokens.Insert(this);
            }
            else
            {
                throw new Exception("Invalid username or password");
            }
        }

        public AuthToken(string TokenId)
        {
            MongoClient client = new MongoClient(Properties.Settings.Default.MongoConnectionString);
            MongoServer server = client.GetServer();
            MongoDatabase db = server.GetDatabase(Properties.Settings.Default.MongoDb);
            MongoCollection<AuthToken> tokens = db.GetCollection<AuthToken>("tokens");
            AuthToken Token = tokens.FindOne(Query<AuthToken>.EQ(e => e.Id, ObjectId.Parse(TokenId)));

            if (Token.ValidUntil < DateTime.Now)
            {
                tokens.Remove(Query<AuthToken>.EQ(e => e.Id, Token.Id));
                throw new Exception("Token expired");
            }
            else
            {
                _Id = Token.Id;
                _Username = Token.Username;
                _ValidUntil = Token.ValidUntil;
            }
        }

        public ObjectId Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        public DateTime ValidUntil
        {
            get { return _ValidUntil; }
            set { _ValidUntil = value; }
        }
    }
}