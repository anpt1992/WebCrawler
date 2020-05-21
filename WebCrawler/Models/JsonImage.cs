using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class Default
    {

        [JsonProperty("load")]
        public string load { get; set; }

        [JsonProperty("w")]
        public string w { get; set; }

        [JsonProperty("h")]
        public string h { get; set; }

        [JsonProperty("src")]
        public string src { get; set; }
    }

    public class Size3column
    {

        [JsonProperty("load")]
        public string load { get; set; }

        [JsonProperty("w")]
        public string w { get; set; }

        [JsonProperty("h")]
        public string h { get; set; }

        [JsonProperty("src")]
        public string src { get; set; }
    }

    public class Size2column
    {

        [JsonProperty("load")]
        public string load { get; set; }

        [JsonProperty("w")]
        public string w { get; set; }

        [JsonProperty("h")]
        public string h { get; set; }

        [JsonProperty("src")]
        public string src { get; set; }
    }

    public class JsonImage
    {
        [JsonProperty("default")]
        public Default df { get; set; }

        [JsonProperty("size3column")]
        public Size3column Size3column { get; set; }

        [JsonProperty("size2column")]
        public Size2column Size2column { get; set; }
    }
}
