using System.ComponentModel.DataAnnotations;
using Artillery.Common;
using Artillery.Data.Models.Enums;

namespace Artillery.DataProcessor.ImportDto;

public class ImportGunsDto
{
    public int ManufacturerId { get; set; }

    [Range(ValidationConstants.GunMinWeight,ValidationConstants.GunMaxWeight)]
    public int GunWeight { get; set; }
    [Range(ValidationConstants.BarrelMinLength,ValidationConstants.BarrelMaxLength)]
    public double BarrelLength { get; set; }

    public int? NumberBuild { get; set; }

    [Range(ValidationConstants.RangeMinLength, ValidationConstants.RangeMaxLength)]
    public int Range { get; set; }

    public string GunType { get; set; } = null!;

    public int ShellId { get; set; }

    public ImportCountryDto[] Countries { get; set; }
}

public class ImportCountryDto
{
    public int Id { get; set; }
}

