using System;
using MongoDB.Bson;

namespace atlasapi.Models
{
    public class UrlShortenedModelDb
    {
        public ObjectId _id { get; set; }

        public int obj { get; set; }

        public string url { get; set; }

        public string short_code { get; set; }

        public DateTime created_date { get; set; }
    }
}
