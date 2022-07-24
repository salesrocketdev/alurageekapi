namespace alurageekapi.Helpers;

public class EcommerceStoreDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string ProductCollectionName { get; set; } = null!;
    public string UserCollectionName { get; set; } = null!;
}