using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Interfaces;
using MongoDB.Driver;

namespace ChatApp.Infrastructure.Repositories
{
    public class MongoMessageRepositories : IMessageRepository
    {
        public readonly IMongoCollection<MongoMessage> _messages;
        public MongoMessageRepositories(IMongoDatabase db) {
            _messages = db.GetCollection<MongoMessage>("Messages");
        }
    }
}
