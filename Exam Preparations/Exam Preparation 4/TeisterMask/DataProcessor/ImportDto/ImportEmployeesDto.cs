using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TeisterMask.DataProcessor.ImportDto;

public class ImportEmployeesDto
{
    [JsonProperty("Username")]
    [MinLength(3)]
    [MaxLength(40)]
    [RegularExpression(@"^[a-zA-Z0-9]+$")]
    [Required]
    public string Username { get; set; } = null!;

    [EmailAddress]
    [Required]
    public string Email { get; set; } = null!;

    [RegularExpression(@"\d{3}-\d{3}-\d{4}")]
    [Required]
    public string Phone { get; set; } = null!;

    public int[] Tasks { get; set; }
}

