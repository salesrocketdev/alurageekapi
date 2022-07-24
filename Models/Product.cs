using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace alurageekapi.Models;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Url { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Category { get; set; } = null!;
    public decimal Price { get; set; }
    public string Description { get; set; } = null!;
}