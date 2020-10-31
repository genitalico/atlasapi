using System;
using System.ComponentModel.DataAnnotations;

namespace atlasapi.Models
{
    public class PostNewUrlModel
    {
        [Required]
        [Url]
        public string url { get; set; }
    }

    public class ResponsePostNewUrlModel
    {
        public string short_url { get; set; }

        public string url { get; set; }
    }
}
