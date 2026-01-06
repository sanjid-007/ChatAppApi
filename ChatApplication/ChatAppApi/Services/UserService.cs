using ChatAppApi.Data;
using ChatAppApi.Models;
using MongoDB.Driver;

namespace ChatAppApi.Services
{
    public class UserService
    {
        private readonly MongoDbContext _context;

        public UserService(MongoDbContext context)
        {
            _context = context;
        }

       
    }
}
