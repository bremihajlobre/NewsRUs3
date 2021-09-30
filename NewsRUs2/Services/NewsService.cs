using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Driver;
using NewsRUs2.Models;

namespace NewsRUs2.Services
{
    public class NewsService
    {
        private readonly IMongoCollection<News> _news;

        public NewsService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _news = database.GetCollection<News>("News");
        }

        public News Create(News neww)
        {
            _news.InsertOne(neww);
            return neww;
        }

        public IList<News> Read() =>
            _news.Find(neew => true).ToList();

        public News Find(string id) =>
            _news.Find(neew => neew.Id == id).SingleOrDefault();

        public void Update(News neww) =>
            _news.ReplaceOne(neew => neew.Id == neww.Id, neww);

        public void Delete(string id) =>
            _news.DeleteOne(neew => neew.Id == id);
    }
}
