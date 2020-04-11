using System;
using System.Threading.Tasks;
using atlasapi.Models;

namespace atlasapi.ITransactions
{
    public interface IAdminTransaction
    {
        Task<Tuple<bool,ResponsePostNewUrlModel>> GenerateShortUrl(string url);
    }
}
