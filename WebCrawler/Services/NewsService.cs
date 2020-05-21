using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Models;
using PagedList;

namespace WebCrawler.Services
{
    public class NewsService
    {
        private readonly IMongoCollection<News> _news;

        public NewsService(INewsDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _news = database.GetCollection<News>(settings.NewsCollectionName);
            var indexOptions = new CreateIndexOptions();
            indexOptions.Unique = true;
            var indexKeys = Builders<News>.IndexKeys.Ascending(i => i.sourceLink);
            var indexModel = new CreateIndexModel<News>(indexKeys, indexOptions);
            _news.Indexes.CreateOne(indexModel);
        }

        public List<News> Get() =>
            _news.Find(news => true).ToList();

        public News Get(string id) =>
            _news.Find<News>(news => news.id == id).FirstOrDefault();
        public List<News> GetByCategory(string catid)
        {
            return _news.Find(news => (news.catid == catid)).ToList();
        }
        public News Create(News news)
        {
            _news.InsertOne(news);
            return news;
        }

        public List<News> SaveAll(List<News> news)
        {
            try
            {
                _news.InsertMany(news);
            }
            catch (Exception)
            {
                throw;
            }
            return news;
        }

        public void Update(string id, News newsIn) =>
            _news.ReplaceOne(news => news.id == id, newsIn);

        public void Remove(News newsIn) =>
            _news.DeleteOne(news => news.id == newsIn.id);

        public void Remove(string id) =>
            _news.DeleteOne(news => news.id == id);
    }
}
