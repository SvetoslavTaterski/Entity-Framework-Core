using Newtonsoft.Json;

namespace ProductShop.DTOs.Import;

public class ImportedProductsDto
{
    [JsonProperty]
    public string Name { get; set; } = null!;

    [JsonProperty]
    public decimal Price { get; set; }

    [JsonProperty]
    public int SellerId { get; set; }

    [JsonProperty]
    public int? BuyerId { get; set; }
}

