using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using atlasapi.Models;
using Microsoft.Extensions.Logging;

namespace atlasapi.mongodb
{
    public interface IMongoTransaction
    {
        Task<Tuple<bool, string>> InsertUrl(UrlShortenedModelDb model, ILogger logger);
        Task<Tuple<bool, string>> FindShortCode(string shortCode);

        Task<Tuple<bool,string>> InsertBulkUrls(List<UrlShortenedModelDb> urls);

        Task<Tuple<bool, List<ResponsePostNewUrlModel>>> GetAll();
    }
}
