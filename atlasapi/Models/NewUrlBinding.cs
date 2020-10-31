using System;
namespace atlasapi.Models
{
    public class PostNewUrlModel
    {
        public string url { get; set; }
    }

    public class ResponsePostNewUrlModel
    {
        public string short_url { get; set; }

        public string url { get; set; }
    }
}
