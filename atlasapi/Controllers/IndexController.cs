using System;
using System.Threading.Tasks;
using atlasapi.ITransactions;
using atlasapi.mongodb;
using atlasapi.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webServiceTools;

namespace atlasapi.Controllers
{
    [ApiController]
    [Route("")]
    public class IndexController : ControllerBase
    {
        #region Const
        private const string _INDEX_REDIRECT_TRACE = "IndexRedirect";
        #endregion

        #region Private Vars
        private readonly ILogger<IndexController> _Logger;
        private readonly IAdminTransaction _AdminTransaction;
        private TransactionCore _TransactionCore;
        #endregion

        #region Ctor
        public IndexController(ILogger<IndexController> logger, IMongoTransaction mongoTransaction)
        {
            this._Logger = logger;

            this._AdminTransaction = new AdminTransaction(mongoTransaction, logger);
        }
        #endregion

        #region REST
        [HttpGet]
        [Route("")]
        public async Task<ContentResult> Index()
        {
            this._TransactionCore = new TransactionCore();

            this._TransactionCore.CommonModel.Correct();

            this._TransactionCore.OkResponse();

            return this._TransactionCore.ContentResult;
        }

        [HttpGet]
        [Route("{ShortCode}")]
        public async Task<IActionResult> Redirect([FromRoute] string ShortCode)
        {
            this._Logger.Log(LogLevel.Information, _INDEX_REDIRECT_TRACE);

            var result = await this._AdminTransaction.ResponseRealUrl(ShortCode);

            if(result.Item1)
            {
                return new RedirectResult(result.Item2);
            }

            this._TransactionCore = new TransactionCore();

            this._TransactionCore.CommonModel.RegisterNotFound();

            this._TransactionCore.OkResponse();

            return this._TransactionCore.ContentResult;
        }
        #endregion
    }
}
