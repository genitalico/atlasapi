using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using atlasapi.Models;

namespace atlasapi.ITransactions
{
    public interface IAdminTransaction
    {
        Task<Tuple<bool, ResponsePostNewUrlModel>> GenerateShortUrl(string url);
        Task<Tuple<bool, string>> ResponseRealUrl(string shortCode);

        Task<Tuple<bool, List<ResponsePostNewUrlModel>>> UploadBulk(Stream stream);
    }
}
