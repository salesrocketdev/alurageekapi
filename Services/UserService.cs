using alurageekapi.Helpers;
using alurageekapi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace alurageekapi.Services;

public class UserService
{
    private readonly IMongoCollection<User> _usersCollection;

    public UserService(IOptions<EcommerceStoreDatabaseSettings> usersStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            usersStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            usersStoreDatabaseSettings.Value.DatabaseName);

        _usersCollection = mongoDatabase.GetCollection<User>(
            usersStoreDatabaseSettings.Value.UserCollectionName);
    }

    public async Task<CommandResult> GetAsync()
    {        
        CommandResult commandResult = new CommandResult();

        try
        {
            var data = await _usersCollection.Find(_ => true).ToListAsync();

            if (data.Count() <= 0) {
                commandResult.Message = "Nenhum Usuário encontrado.";
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
    
    public async Task<User?> Get(string email, string password) =>
        await _usersCollection.Find(x => x.Email == email && x.Password == password).FirstOrDefaultAsync();

    public async Task Register(User newUser) =>
        await _usersCollection.InsertOneAsync(newUser);
}