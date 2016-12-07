using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Model;
using MongoDB.Driver.Builders;

namespace Service
{
    public static class MongoDBService
    {
        static IMongoDatabase db;
        static MongoDBService() {
            var set = new MongoClientSettings();
            var cred = new List<MongoCredential>() { MongoCredential.CreateCredential("shtx", "shtx", "shyr021191") };
            var server1 = new MongoServerAddress("172.20.67.110", 27017);
            //var server2 = new MongoServerAddress("172.20.67.111", 27017);
            //var server3 = new MongoServerAddress("172.20.67.111", 27018);
            //var server4 = new MongoServerAddress("172.20.67.111", 27019);
            var servers = new List<MongoServerAddress>() { server1 };
            //var servers = new List<MongoServerAddress>() { server1, server2, server3, server4 };
            set.Credentials = cred;
            set.Servers = servers;
            set.ConnectionMode = ConnectionMode.ReplicaSet;
            set.ReplicaSetName = "tongxin";
            set.ReadPreference = new ReadPreference(ReadPreferenceMode.SecondaryPreferred);
            var client = new MongoClient(set);
            //var client = new MongoClient(conn);
            db = client.GetDatabase("shtx");
        }
        //private static string conn = @"mongodb://shtx:shyr021191@172.20.67.110:27017/shtx?readPreference=secondary";

        public static Price GetLastPricesByProductId(int productId)
        {
            Price price = null;

            var collection = db.GetCollection<BsonDocument>("prices");
            var filter = Builders<BsonDocument>.Filter.Eq("productId", productId);
            var data = collection.Find(filter).Sort(Builders<BsonDocument>.Sort.Descending("addDate")).FirstOrDefault();

            //var data = collection.WithReadPreference(ReadPreference.SecondaryPreferred)
            //    .Find(filter).Sort(Builders<BsonDocument>.Sort.Descending("addDate")).ToList();

            if (data != null)
            {
                price = new Price();
                price.MarketID = data["marketId"].ToInt32();
                price.ProductID = data["productId"].ToInt32();
                price.HPrice = data["high"].IsBsonNull ? "" : data["high"].ToString();
                price.LPrice = data["low"].IsBsonNull ? "" : data["low"].ToString();
                price.APrice = data["average"].IsBsonNull ? "" : data["average"].ToString();
                price.PriceChange = data["change"].IsBsonNull ? "" : data["change"].ToString(); ;
                price.AddDate = Convert.ToDateTime(data["addDate"]).ToLocalTime();
            }

            return price;
        }

        public static List<Price> GetHostoryPrices(int productId, DateTime start, DateTime end)
        {
            end = end.AddDays(1);
            var list = new List<Price>();

            var collection = db.GetCollection<BsonDocument>("prices");

            var filterBuilder = Builders<BsonDocument>.Filter;

            var filter = filterBuilder.Gt("addDate", start) & filterBuilder.Lte("addDate", end) & filterBuilder.Eq("productId", productId);
            var data = collection.Find(filter)
                .Sort(Builders<BsonDocument>.Sort.Descending("addDate")).ToList();

            foreach (var d in data)
            {
                var price = new Price();
                price.MarketID = d["marketId"].ToInt32();
                price.ProductID = d["productId"].ToInt32();
                price.HPrice = d["high"].IsBsonNull ? "" : d["high"].ToString();
                price.LPrice = d["low"].IsBsonNull ? "" : d["low"].ToString();
                price.APrice = d["average"].IsBsonNull ? "" : d["average"].ToString();
                price.PriceChange = d["change"].IsBsonNull ? "" : d["change"].ToString();
                price.AddDate = Convert.ToDateTime(d["addDate"]).ToLocalTime();
                list.Add(price);
            }

            return list;
        }
    }
}
