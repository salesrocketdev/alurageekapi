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

    public async Task<CommandResult> GetAsync() 
    {
        CommandResult commandResult = new CommandResult();

        try
        {
            var data = await _productsCollection.Find(_ => true).ToListAsync();

            if (data.Count() <= 0) {
                commandResult.Message = "Nenhum produto encontrado.";
            } 
            else 
            {
                commandResult.Message = data.Count().ToString();
            }

            commandResult.Data = data;

            return commandResult;
        }
        catch (System.Exception e)
        {         
            commandResult.Message = e.ToString();
            return commandResult;
        }
    }

    public async Task<CommandResult?> GetAsync(string id)
    {
    CommandResult commandResult = new CommandResult();

        try
        {
            var data = await _productsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();            

            commandResult.Data = data;

            return commandResult;
        }
        catch (System.Exception e)
        {         
            commandResult.Message = e.ToString();
            return commandResult;
        }
    }

    public async Task<CommandResult> GetByTitleAsync(string title) 
    {
        CommandResult commandResult = new CommandResult();

        try
        {
            var data = await _productsCollection.Find(x => x.Title.ToLower().Contains(title.ToLower())).ToListAsync();

            if (data.Count() <= 0) {
                commandResult.Message = "Nenhum produto encontrado.";
            } 
            else 
            {
                commandResult.Message = data.Count().ToString();
            }
            
            commandResult.Data = data;
            
            return commandResult;
        }
        catch (System.Exception e)
        {         
            commandResult.Message = e.ToString();      
            return commandResult;
        }
    }

    public async Task<CommandResult> GetCategoryAsync(string category) 
    {
        CommandResult commandResult = new CommandResult();

        try
        {
            var data = await _productsCollection.Find(x => x.Category.ToLower().Contains(category.ToLower())).ToListAsync();

            commandResult.Message = data.Count().ToString();

            if (data.Count() <= 0) {
                commandResult.Message = "Nenhum produto encontrado.";
            } 
            else 
            {
                commandResult.Message = data.Count().ToString();
            }

            commandResult.Data = data;
            
            return commandResult;
        }
        catch (System.Exception e)
        {         
            commandResult.Message = e.ToString();      
            return commandResult;
        }        
    }
    
    public async Task CreateAsync(Product newProduct) =>
        await _productsCollection.InsertOneAsync(newProduct);

    public async Task UpdateAsync(string id, Product updatedProduct) =>
        await _productsCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

    public async Task RemoveAsync(string id) =>
        await _productsCollection.DeleteOneAsync(x => x.Id == id);
}