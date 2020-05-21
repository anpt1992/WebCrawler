using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace WebCrawler.Models
{
    public class NewsByCategory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public bool isEn { get; set; }
        public string title { get; set; }
        public string source { get; set; }
        public string thumb { get; set; }
        public string sourceLink { get; set; }        
        public string linkVideo { get; set; }
        public string catid { get; set; }
        public string contentText { get; set; }        
        public DateTime createdDate { get; set; }       
        public bool isVideo { get; set; }
        public DateTime publishedDate { get; set; }
        public int nextpage { get; set; }
    }
}
