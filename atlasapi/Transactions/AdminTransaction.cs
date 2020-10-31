using System;
using System.Threading.Tasks;
using atlasapi.Controllers;
using atlasapi.Helpers;
using atlasapi.ITransactions;
using atlasapi.Models;
using atlasapi.mongodb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace atlasapi.Transactions
{
    public class AdminTransaction : IAdminTransaction
    {
        #region Const
        private const string _GENERATE_SHORT_URL_TRACE = "AdminTransaction, GenerateShortUrl";
        #endregion

        #region PrivateVars
        private IMongoTransaction _MongoTransaction;
        private IConfiguration _Configuration;
        private ILogger _Logger;
        #endregion

        #region Ctor
        public AdminTransaction(IMongoTransaction mongoTransaction, IConfiguration configuration, ILogger<AdminController> logger)
        {
            this._MongoTransaction = mongoTransaction;
            this._Configuration = configuration;
            this._Logger = logger;
        }

        public AdminTransaction(IMongoTransaction mongoTransaction, ILogger<IndexController> logger)
        {
            this._MongoTransaction = mongoTransaction;
            this._Logger = logger;
        }
        #endregion

        #region PublicMethods
        public async Task<Tuple<bool, ResponsePostNewUrlModel>> GenerateShortUrl(string url)
        {
            try
            {
                this._Logger.Log(LogLevel.Information, _GENERATE_SHORT_URL_TRACE);

                var sizeCode = Convert.ToInt32(this._Configuration["SizeCode"].ToString());

                var doc = new UrlShortenedModelDb()
                {
                    url = url,
                    obj = (int)OBJ_DOCUMENT.SHORT_URL,
                    short_code = Tools.GetAlphanumericRandom(sizeCode),
                    created_date = DateTime.UtcNow
                };

                var result = await this._MongoTransaction.InsertUrl(doc, this._Logger);

                if (result.Item1)
                {
                    var response = new ResponsePostNewUrlModel()
                    {
                        short_url = doc.short_code,
                        url = url
                    };

                    return new Tuple<bool, ResponsePostNewUrlModel>(true, response);
                }

                return new Tuple<bool, ResponsePostNewUrlModel>(false, null);

            }
            catch (Exception ex)
            {
                this._Logger.Log(LogLevel.Critical, ex, _GENERATE_SHORT_URL_TRACE);

                return new Tuple<bool, ResponsePostNewUrlModel>(false, null);
            }
        }

        public async Task<Tuple<bool, string>> ResponseRealUrl(string shortCode)
        {
            try
            {
                var result = await this._MongoTransaction.FindShortCode(shortCode);

                return result;
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "");
            }
        }
        #endregion
    }
}
