using System;
using System.Threading.Tasks;
using atlasapi.Helpers;
using atlasapi.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace atlasapi.mongodb
{
    public class MongoTransaction : IMongoTransaction
    {
        #region PrivateVars
        private MongoClient _MongoClient;
        private IMongoDatabase _MongoDatabase;
        private string _MongoCollection;
        private int _SizeCode;
        #endregion

        public MongoTransaction(string cs,string dbName,string collection,int sizeCode)
        {
            this._MongoCollection = collection;
            this._SizeCode = sizeCode;

            this._MongoClient = new MongoClient(cs);

            this._MongoDatabase = this._MongoClient.GetDatabase(dbName);
        }

        #region PublicMethods
        public async Task<bool> InsertUrl(string url)
        {
            try
            {
                var collection = this._MongoDatabase.GetCollection<UrlShortenedModelDb>(this._MongoCollection);

                var doc = new UrlShortenedModelDb()
                {
                    url = url,
                    obj = (int)OBJ_DOCUMENT.SHORT_URL,
                    short_code = Tools.GetAlphanumericRandom(this._SizeCode),
                    created_date = DateTime.UtcNow
                };

                await collection.InsertOneAsync(doc);

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
