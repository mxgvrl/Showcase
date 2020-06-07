using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace AppleShowcase.Data.Models
{
    public class UserService
    {
        public IMongoCollection<User> Users;

        public UserService()
        {
            const string connectionString = "mongodb://localhost:27017/ShowcaseDB";
            var connection = new MongoUrlBuilder(connectionString);
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(connection.DatabaseName);
            Users = database.GetCollection<User>("users");
        }
    }
}