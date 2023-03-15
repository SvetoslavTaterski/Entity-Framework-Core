using Newtonsoft.Json;

namespace ProductShop.DTOs.Import;

public class ImportedCategoriesDto
{
    [JsonProperty("name")]
    public string? Name { get; set; }
}

