using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto;

public class ImportDeparmentDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(25)]
    public string Name { get; set; } = null!;

    public ImportCellsDto[] Cells { get; set; }
}

public class ImportCellsDto
{
    [Required]
    [Range(1,1000)]
    public int CellNumber { get; set; }

    [Required]
    public bool HasWindow { get; set; }
}