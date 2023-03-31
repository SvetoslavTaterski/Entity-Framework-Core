using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto;

public class ImportPrisonersDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(20)]
    public string FullName { get; set; } = null!;

    [Required]
    [RegularExpression(@"^The [A-Z][a-z]*$")]
    public string Nickname { get; set; } = null!;

    [Required]
    [Range(18,65)]
    public int Age { get; set; }

    [Required] 
    public string IncarcerationDate { get; set; } = null!;

    public string? ReleaseDate { get; set; }

    [Range(0,double.PositiveInfinity)]
    public decimal? Bail { get; set; }

    public int? CellId { get; set; }

    public ImportMailsDto[] Mails { get; set; }
}

public class ImportMailsDto
{
    [Required]
    public string Description { get; set; } = null!;

    [Required]
    public string Sender { get; set; } = null!;

    [Required]
    [RegularExpression(@"^[A-Za-z\d ]*str.$")]
    public string Address { get; set; } = null!;
}

