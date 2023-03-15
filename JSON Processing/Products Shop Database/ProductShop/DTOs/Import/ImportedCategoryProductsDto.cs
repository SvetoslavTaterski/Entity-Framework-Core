using System.Reflection.Metadata.Ecma335;
using Newtonsoft.Json;

namespace ProductShop.DTOs.Import;

public class ImportedCategoryProductsDto
{
    [JsonProperty]
    public int CategoryId { get; set; }

    [JsonProperty]
    public int ProductId { get; set; }
}

