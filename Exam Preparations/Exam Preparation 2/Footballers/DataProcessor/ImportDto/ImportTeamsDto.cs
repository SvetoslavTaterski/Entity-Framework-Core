using System.ComponentModel.DataAnnotations;
using Footballers.Common;

namespace Footballers.DataProcessor.ImportDto;

public class ImportTeamsDto
{
    [MinLength(ValidationConstants.TeamNameMinLength)]
    [MaxLength(ValidationConstants.TeamNameMaxLength)]
    [RegularExpression(@"[a-zA-z.\-\d\s]+$")]
    public string Name { get; set; } = null!;

    [MaxLength(ValidationConstants.TeamNationalityMaxLength)]
    [MinLength(ValidationConstants.TeamNationalityMinLength)]
    public string Nationality { get; set; } = null!;

    public int Trophies { get; set; }

    public int[] Footballers { get; set; }
}


