using System.Threading.Tasks;
using atlasapi.ITransactions;
using atlasapi.Models;
using atlasapi.mongodb;
using atlasapi.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using webServiceTools;

namespace atlasapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        #region MyRegion
        private const string _POST_NEW_URL_TRACE = "PostNewUrl";
        #endregion

        #region PrivateVars
        private readonly ILogger<AdminController> _Logger;
        private readonly IMongoTransaction _MongoTransaction;
        private TransactionCore _TransactionCore;
        private IAdminTransaction _AdminTransaction;
        #endregion

        #region Ctor
        public AdminController(ILogger<AdminController> logger, IMongoTransaction mongoTransaction, IConfiguration configuration)
        {
            this._Logger = logger;

            this._MongoTransaction = mongoTransaction;

            this._AdminTransaction = new AdminTransaction(this._MongoTransaction, configuration,logger);
        }
        #endregion

        #region REST
        [HttpPost]
        [Route("AddUrl")]
        public async Task<ContentResult> PostNewUrl([FromBody] PostNewUrlModel model)
        {
            this._Logger.Log(LogLevel.Information, _POST_NEW_URL_TRACE);

            var result = await this._AdminTransaction.GenerateShortUrl(model.url);

            this._TransactionCore = new TransactionCore();

            if (result.Item1)
            {
                this._TransactionCore.CommonModel.RegisterCreated();

                this._TransactionCore.CommonModel.content = result.Item2;

                this._TransactionCore.OkResponse();

                return this._TransactionCore.ContentResult;
            }

            this._TransactionCore.CommonModel.InternalError();
            this._TransactionCore.OkResponse();

            this._Logger.Log(LogLevel.Information, _POST_NEW_URL_TRACE);

            return this._TransactionCore.ContentResult;
        }
        #endregion
    }
}
