﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using atlasapi.Helpers;
using atlasapi.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace atlasapi.mongodb
{
    public class MongoTransaction : IMongoTransaction
    {
        #region Const
        private const string _INSERT_NEW_URL_TRACE = "MongoTransaction, InsertUrl";
        #endregion

        #region PrivateVars
        private MongoClient _MongoClient;
        private IMongoDatabase _MongoDatabase;
        private string _MongoCollection;
        #endregion

        public MongoTransaction(string cs, string dbName, string collection)
        {
            this._MongoCollection = collection;

            this._MongoClient = new MongoClient(cs);

            this._MongoDatabase = this._MongoClient.GetDatabase(dbName);
        }

        #region PublicMethods
        public async Task<Tuple<bool, string>> InsertUrl(UrlShortenedModelDb model, ILogger logger)
        {
            try
            {
                logger.Log(LogLevel.Information, _INSERT_NEW_URL_TRACE);

                var collection = this._MongoDatabase.GetCollection<UrlShortenedModelDb>(this._MongoCollection);

                await collection.InsertOneAsync(model);

                return new Tuple<bool, string>(true, model.short_code);
            }
            catch (MongoWriteException ex)
            {
                logger.Log(LogLevel.Critical, ex, _INSERT_NEW_URL_TRACE);

                return new Tuple<bool, string>(false, "");
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Critical, ex, _INSERT_NEW_URL_TRACE);

                return new Tuple<bool, string>(false, "");
            }
        }

        public async Task<Tuple<bool, string>> FindShortCode(string shortCode)
        {
            try
            {
                var filter = Builders<UrlShortenedModelDb>.Filter.Eq("obj", (int)OBJ_DOCUMENT.SHORT_URL);
                var filter2 = Builders<UrlShortenedModelDb>.Filter.Eq("short_code", shortCode);
                var filter3 = Builders<UrlShortenedModelDb>.Filter.And(filter, filter2);

                var collection = this._MongoDatabase.GetCollection<UrlShortenedModelDb>(this._MongoCollection);

                var document = await collection.Find(filter3).FirstOrDefaultAsync();

                return new Tuple<bool, string>(true, document.url);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "");
            }
        }

        public async Task<Tuple<bool, string>> InsertBulkUrls(List<UrlShortenedModelDb> urls)
        {
            try
            {
                var collection = this._MongoDatabase.GetCollection<UrlShortenedModelDb>(this._MongoCollection);

                await collection.InsertManyAsync(urls);

                return new Tuple<bool, string>(true, "");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "");
            }
        }

        public async Task<Tuple<bool, List<ResponsePostNewUrlModel>>> GetAll()
        {
            try
            {
                var collection = this._MongoDatabase.GetCollection<UrlShortenedModelDb>(this._MongoCollection);

                var filter = Builders<UrlShortenedModelDb>.Filter.Eq("obj", (int)OBJ_DOCUMENT.SHORT_URL);
                var documents = await collection.FindAsync(filter);

                var documents2 = await documents.ToListAsync();

                var documents3 = new List<ResponsePostNewUrlModel>();

                foreach(var doc in documents2)
                {
                    var model = new ResponsePostNewUrlModel
                    {
                        url = doc.url,
                        short_url = doc.short_code
                    };

                    documents3.Add(model);
                }

                return new Tuple<bool, List<ResponsePostNewUrlModel>>(true, documents3);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, List<ResponsePostNewUrlModel>>(false, new List<ResponsePostNewUrlModel>());
            }
        }
        #endregion
    }
}
