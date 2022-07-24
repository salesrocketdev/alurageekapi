using alurageekapi.Helpers;
using alurageekapi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace alurageekapi.Services;

public class ProductsService
{
    private readonly IMongoCollection<Product> _productsCollection;

    public ProductsService(IOptions<EcommerceStoreDatabaseSettings> productStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            productStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            productStoreDatabaseSettings.Value.DatabaseName);

        _productsCollection = mongoDatabase.GetCollection<Product>(
            productStoreDatabaseSettings.Value.ProductCollectionName);
    }

    public async Task<List<Product>> GetAsync() =>
        await _productsCollection.Find(_ => true).ToListAsync();

    public async Task<Product?> GetAsync(string id) =>
        await _productsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<List<Product>> GetByTitleAsync(string title) =>
        await _productsCollection.Find(x => x.Title.ToLower().Contains(title.ToLower())).ToListAsync();

    public async Task<List<Product>> GetCategoryAsync(string category) =>
        await _productsCollection.Find(x => x.Category.ToLower().Contains(category.ToLower())).ToListAsync();
    
    public async Task CreateAsync(Product newProduct) =>
        await _productsCollection.InsertOneAsync(newProduct);

    public async Task UpdateAsync(string id, Product updatedProduct) =>
        await _productsCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

    public async Task RemoveAsync(string id) =>
        await _productsCollection.DeleteOneAsync(x => x.Id == id);
}