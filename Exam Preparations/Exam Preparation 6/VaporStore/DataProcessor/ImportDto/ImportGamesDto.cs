using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor.ImportDto;

public class ImportGamesDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    [Range(0,double.PositiveInfinity)]
    public decimal Price { get; set; }

    [Required] 
    public string ReleaseDate { get; set; } = null!;

    [Required] 
    public string Developer { get; set; } = null!;

    [Required] 
    public string Genre { get; set; } = null!;

    [Required] 
    public string[] Tags { get; set; } = null!;
}

