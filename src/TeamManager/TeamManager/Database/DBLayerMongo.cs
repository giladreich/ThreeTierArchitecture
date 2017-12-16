﻿using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using log4net;
using TeamManager.Models.ResourceData;
using TeamManager.Utilities;

namespace TeamManager.Database
{
    public class DBLayerMongo : IDataLayer
    {
        private static readonly ILog Log = Logger.GetLogger();


        // get the mlab db servers credentials from environment variables (set them with .env.bat)
        internal static readonly string MLAB_USERNAME;
        internal static readonly string MLAB_PASSWORD;
        internal static readonly string MLAB_URI;
        internal static readonly string MLAB_PORT;
        internal static readonly string MLAB_DATABASE_NAME;

        private static readonly string DatabaseName;
        private static readonly string ConnectionString;

        private static MongoClient Client { get; set; }
        public static IMongoDatabase Database { get; private set; }
        public static IMongoCollection<Team> TeamCollection { get; set; }
        public static IMongoCollection<Player> PlayerCollection { get; set; }

        private const string TeamsCollectionName = "team";
        private const string PlayersCollectionName = "player";

        private const int TimeoutMilisec = 3000;

        static DBLayerMongo()
        {
#if MONGO_DB_LOCAL
            Log.Info("Using Mongo-Db local server connection.");
            // connect to local mongodb server
            DatabaseName     = "teamplayer";
            ConnectionString = "mongodb://localhost:27017";
#else
            Log.Info("Using Mongo-Db online server connection.");

            try
            {
                MLAB_USERNAME      = Environment.GetEnvironmentVariable("MLAB_USERNAME");
                MLAB_PASSWORD      = Environment.GetEnvironmentVariable("MLAB_PASSWORD");
                MLAB_URI           = Environment.GetEnvironmentVariable("MLAB_URI");
                MLAB_PORT          = Environment.GetEnvironmentVariable("MLAB_PORT");
                MLAB_DATABASE_NAME = Environment.GetEnvironmentVariable("MLAB_DATABASE_NAME");
            }
            catch (Exception e)
            {
                Log.Error("cctor - Failed to retrieve environment variables => ", e);
            }

            // connect to mLab mongodb server
            DatabaseName     = MLAB_DATABASE_NAME;
            ConnectionString = "mongodb://" + MLAB_USERNAME + ":" + MLAB_PASSWORD + "@" + MLAB_URI + ":" + MLAB_PORT + "/" + MLAB_DATABASE_NAME;
#endif
        }

        public DBLayerMongo()
        {
            ConnectDB();
        }

        public void ConnectDB()
        {
            try
            {
                Client = new MongoClient(ConnectionString);

                Database = Client.GetDatabase(DatabaseName);
                TeamCollection = Database.GetCollection<Team>(TeamsCollectionName);
                PlayerCollection = Database.GetCollection<Player>(PlayersCollectionName);
            }
            catch (Exception e)
            {
                Log.Fatal("Failed to initialize or create connection to mongodb => ", e);
                throw;
            }
        }

        public bool CreatePlayer(string name, string teamId)
        {
            try
            {
                PlayerCollection.InsertOne(new Player(name, teamId));
                return true;
            }
            catch (TimeoutException e)
            {
                Log.Error("Received time out for creating player => ", e);
                return false;
            }
            catch (Exception e)
            {
                Log.Error("Failed to create player => ", e);
                return false;
            }
        }

        public bool CreateTeam(string name)
        {
            try
            {
                TeamCollection.InsertOne(new Team(name));
                return true;
            }
            catch (TimeoutException e)
            {
                Log.Error("Received time out for creating team => ", e);
                return false;
            }
            catch (Exception e)
            {
                Log.Error("Failed to create team => ", e);
                return false;
            }
        }

        public bool DeletePlayer(string id)
        {
            try
            {
                PlayerCollection.DeleteOne(a => a.Id == id);
                return true;
            }
            catch (TimeoutException e)
            {
                Log.Error("Received time out for deleting player => ", e);
                return false;
            }
            catch (Exception e)
            {
                Log.Error("Failed to delete player => ", e);
                return false;
            }
        }

        public bool DeleteTeam(string id)
        {
            try
            {
                TeamCollection.DeleteOne(a => a.Id == id);
                return true;
            }
            catch (TimeoutException e)
            {
                Log.Error("Received time out for deleting team => ", e);
                return false;
            }
            catch (Exception e)
            {
                Log.Error("Failed to delete team => ", e);
                return false;
            }
        }

        public Player ReadPlayer(string id)
        {
            try
            {
                var results = PlayerCollection.Find(p => p.Id == id);
                Player player = results.First<Player>();
                return player;
            }
            catch (TimeoutException e)
            {
                Log.Error("Received time out for reading player => ", e);
                return null;
            }
            catch (Exception e)
            {
                Log.Error("Failed to read player => ", e);
                return null;
            }
        }

        public Team ReadTeam(string id)
        {
            try
            {
                var results = TeamCollection.Find(t => t.Id == id);
                Team team = results.First<Team>();
                return team;
            }
            catch (TimeoutException e)
            {
                Log.Error("Received time out for reading team => ", e);
                return null;
            }
            catch (Exception e)
            {
                Log.Error("Failed to read team => ", e);
                return null;
            }
        }

        public List<Player> ShowPlayers(string teamId)
        {
            try
            {
                List<Player> players = PlayerCollection.Find(p => p.TeamId == teamId).ToList();
                return players;
            }
            catch (TimeoutException e)
            {
                Log.Error("Received time out for showing players => ", e);
                return null;
            }
            catch (Exception e)
            {
                Log.Error("Failed to retrieve players => ", e);
                return null;
            }
        }

        public bool UpdatePlayer(string id, string name)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var update = Builders<BsonDocument>.Update.Set("Name", name);

            try
            {
                var collection = Database.GetCollection<BsonDocument>(PlayersCollectionName);
                collection.UpdateOne(filter, update);
                return true;
            }
            catch (TimeoutException e)
            {
                Log.Error("Received time out for updating player => ", e);
                return false;
            }
            catch (Exception e)
            {
                Log.Error("Failed to update player => ", e);
                return false;
            }
        }

        public async Task<bool> UpdatePlayerAsync(string id, string name)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var update = Builders<BsonDocument>.Update.Set("Name", name);

            try
            {
                var collection = Database.GetCollection<BsonDocument>(PlayersCollectionName);
                await collection.UpdateOneAsync(filter, update).TimeoutAfter(TimeoutMilisec);
                return true;
            }
            catch (TimeoutException e)
            {
                Log.Error("Received time out for updating player async => ", e);
                return false;
            }
            catch (Exception e)
            {
                Log.Error("Failed to update player async => ", e);
                return false;
            }
        }

        public bool UpdatePlayer(string id, string teamId, string name)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var update = Builders<BsonDocument>.Update.Set("Name", name);

            try
            {
                var collection = Database.GetCollection<BsonDocument>(PlayersCollectionName);
                collection.UpdateOne(filter, update);
                return true;
            }
            catch (TimeoutException e)
            {
                Log.Error("Received time out for updating player => ", e);
                return false;
            }
            catch (Exception e)
            {
                Log.Error("Failed to update player => ", e);
                return false;
            }
        }

        public async Task<bool> UpdatePlayerAsync(string id, string teamId, string name)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var update = Builders<BsonDocument>.Update.Set("Name", name);

            try
            {
                var collection = Database.GetCollection<BsonDocument>(PlayersCollectionName);
                await collection.UpdateOneAsync(filter, update).TimeoutAfter(TimeoutMilisec);
                return true;
            }
            catch (TimeoutException e)
            {
                Log.Error("Received time out for updating player async => ", e);
                return false;
            }
            catch (Exception e)
            {
                Log.Error("Failed to update player async => ", e);
                return false;
            }
        }

        public bool UpdateTeam(string id, string name)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var update = Builders<BsonDocument>.Update.Set("Name", name);

            try
            {
                var collection = Database.GetCollection<BsonDocument>(TeamsCollectionName);
                collection.UpdateOne(filter, update);
                return true;
            }
            catch (TimeoutException e)
            {
                Log.Error("Received time out for updating team => ", e);
                return false;
            }
            catch (Exception e)
            {
                Log.Error("Failed to update team => ", e);
                return false;
            }
        }

        public async Task<bool> UpdateTeamAsync(string id, string name)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var update = Builders<BsonDocument>.Update.Set("Name", name);

            try
            {
                var collection = Database.GetCollection<BsonDocument>(TeamsCollectionName);
                await collection.UpdateOneAsync(filter, update).TimeoutAfter(TimeoutMilisec);
                return true;
            }
            catch (TimeoutException e)
            {
                Log.Error("Received time out for updating team async => ", e);
                return false;
            }
            catch (Exception e)
            {
                Log.Error("Failed to update team async => ", e);
                return false;
            }
        }

        public List<Player> Players()
        {
            try
            {
                List<Player> players = PlayerCollection.Find(_ => true).ToList();
                return players;
            }
            catch (TimeoutException e)
            {
                Log.Error("Received time out for retrieving players => ", e);
                return null;
            }
            catch (Exception e)
            {
                Log.Error("Failed to retrieve players => ", e);
                return null;
            }
        }

        public List<Team> Teams()
        {
            try
            {
                List<Team> teams = TeamCollection.Find(_ => true).ToList();
                return teams;
            }
            catch (TimeoutException e)
            {
                Log.Error("Received time out for retrieving teams => ", e);
                return null;
            }
            catch (Exception e)
            {
                Log.Error("Failed to retrieve teams => ", e);
                return null;
            }
        }

        public bool ChangePlayerTeam(string playerId, string teamId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", playerId);
            var update = Builders<BsonDocument>.Update.Set("TeamId", teamId);

            try
            {
                var collection = Database.GetCollection<BsonDocument>(PlayersCollectionName);
                collection.UpdateOne(filter, update);
                return true;
            }
            catch (TimeoutException e)
            {
                Log.Error("Received time out for changing player team => ", e);
                return false;
            }
            catch (Exception e)
            {
                Log.Error("Failed to change player team => ", e);
                return false;
            }
        }


    }
}
