using MongoDB.Driver;

namespace Phaedra.Server.Data
{
    public class DocumentContext : IDocumentContext
    {
        public IMongoDatabase _database;
        public DocumentContext(IConfiguration configuration) 
        {
            var connectionString = configuration.GetRequiredSection("DbContextSettings:DocumentDbConnection")
                .Value ?? throw new ArgumentNullException("DocumentDB Connection string not found");
            _database = new MongoClient(connectionString).GetDatabase("PhaedraDocumentDb");
        }
 
        public IMongoCollection<T> GetCollection<T>()
        {
            return _database.GetCollection<T>(typeof(T).Name);
        }

    }
}
