using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using atlasapi.ITransactions;
using atlasapi.Models;
using atlasapi.mongodb;
using atlasapi.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using webServiceTools;
using System;
using System.IO;
using System.Text;

namespace atlasapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        #region Const
        private const string _POST_NEW_URL_TRACE = "PostNewUrl";
        #endregion

        #region PrivateVars
        private readonly ILogger<AdminController> _Logger;
        private readonly IMongoTransaction _MongoTransaction;
        private TransactionCore _TransactionCore;
        private readonly IAdminTransaction _AdminTransaction;
        #endregion

        #region Ctor
        public AdminController(ILogger<AdminController> logger, IMongoTransaction mongoTransaction, IConfiguration configuration)
        {
            this._Logger = logger;

            this._MongoTransaction = mongoTransaction;

            this._AdminTransaction = new AdminTransaction(this._MongoTransaction, configuration, logger);
        }
        #endregion

        #region REST
        [HttpPost]
        [Route("AddUrl")]
        public async Task<ContentResult> PostNewUrl([FromBody] PostNewUrlModel model)
        {
            this._Logger.Log(LogLevel.Information, _POST_NEW_URL_TRACE);

            this._TransactionCore = new TransactionCore();

            if (!ModelState.IsValid)
            {
                this._TransactionCore.CommonModel.InvalidModel();
                this._TransactionCore.OkResponse();

                this._Logger.Log(LogLevel.Information, _POST_NEW_URL_TRACE);

                return this._TransactionCore.ContentResult;
            }

            var result = await this._AdminTransaction.GenerateShortUrl(model.url);

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

        [HttpPost]
        [Route("bulkUrl")]
        public async Task<ContentResult> BulkUrl(List<IFormFile> file)
        {
            this._TransactionCore = new TransactionCore();

            if (file.Count < 1)
            {
                this._TransactionCore.CommonModel.InvalidModel();
                this._TransactionCore.OkResponse();

                return this._TransactionCore.ContentResult;
            }

            Console.WriteLine(file[0].ContentType);

            if (file[0].ContentType != "text/plain")
            {
                this._TransactionCore.CommonModel.InvalidModel();
                this._TransactionCore.OkResponse();

                return this._TransactionCore.ContentResult;
            }

            var result = await this._AdminTransaction.UploadBulk(file[0].OpenReadStream());

            if (result.Item1)
            {
                this._TransactionCore.CommonModel.RegisterCreated();

                this._TransactionCore.CommonModel.content = result.Item2;

                this._TransactionCore.OkResponse();

                return this._TransactionCore.ContentResult;
            }

            this._TransactionCore.CommonModel.InternalError();
            this._TransactionCore.OkResponse();

            return this._TransactionCore.ContentResult;
        }

        [HttpGet]
        [Route("list")]
        public async Task<ContentResult> GetAll()
        {
            this._TransactionCore = new TransactionCore();

            var result = await this._AdminTransaction.GetAll();


            if (result.Item1)
            {
                this._TransactionCore.CommonModel.Correct();

                this._TransactionCore.CommonModel.content = result.Item2;

                this._TransactionCore.OkResponse();

                return this._TransactionCore.ContentResult;
            }

            this._TransactionCore.CommonModel.InternalError();
            this._TransactionCore.OkResponse();

            return this._TransactionCore.ContentResult;
        }
        #endregion
    }
}
