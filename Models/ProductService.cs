using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace AppleShowcase.Data.Models {
    public class ProductService {
        IGridFSBucket gridFS;
        IMongoCollection<Product> products;
        public ProductService()
        {
            const string connectionString = "mongodb://localhost:27017/ShowcaseDB";
            var connection = new MongoUrlBuilder(connectionString);
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(connection.DatabaseName);
            gridFS = new GridFSBucket(database);
            products = database.GetCollection<Product>("products");
        }
        
        public async Task<IEnumerable<Product>> GetProducts(string name)
        {
            var builder = new FilterDefinitionBuilder<Product>();
            var filter = builder.Empty; 
            if (!string.IsNullOrWhiteSpace(name))
            {
                filter &= builder.Regex("name", new BsonRegularExpression(name));
            }
            return await products.Find(filter).ToListAsync();
        }
        public async Task<Product> GetProduct(string id)
        {
            return await products.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }
        public async Task Create(Product p)
        {
            await products.InsertOneAsync(p);
        }

        public async Task Update(Product p)
        {
            await products.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(p.Id)), p);
        }

        public async Task Remove(string id)
        {
            await products.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        } 
        
        public async Task<byte[]> GetImage(string id)
        {
            return await gridFS.DownloadAsBytesAsync(new ObjectId(id));
        }

        public async Task StoreImage(string id, Stream imageStream, string imageName)
        {
            var p = await GetProduct(id);
            if (p.HasImage())
            {
                await gridFS.DeleteAsync(new ObjectId(p.imageID));
            }
            var imageId = await gridFS.UploadFromStreamAsync(imageName, imageStream);
            p.imageID = imageId.ToString();
            var filter = Builders<Product>.Filter.Eq("_id", new ObjectId(p.Id));
            var update = Builders<Product>.Update.Set("imageID", p.imageID);
            await products.UpdateOneAsync(filter, update);
        }
    }
}