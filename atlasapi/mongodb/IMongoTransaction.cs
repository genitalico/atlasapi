using System;
using System.Threading.Tasks;

namespace atlasapi.mongodb
{
    public interface IMongoTransaction
    {
        Task<bool> InsertUrl(string url);
    }
}
