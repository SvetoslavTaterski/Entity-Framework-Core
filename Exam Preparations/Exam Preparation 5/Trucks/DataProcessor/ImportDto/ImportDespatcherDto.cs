using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ImportDto;

[XmlType("Despatcher")]
public class ImportDespatcherDto
{
    [Required]
    [MinLength(2)]
    [MaxLength(40)]
    [XmlElement("Name")]
    public string Name { get; set; } = null!;

    [XmlElement("Position")]
    public string? Position { get; set; }

    [XmlArray("Trucks")]
    public ImportTrucksDto[] Trucks { get; set; }
}

[XmlType("Truck")]
public class ImportTrucksDto
{
    [XmlElement("RegistrationNumber")]
    [MinLength(8)]
    [MaxLength(8)]
    [RegularExpression(@"[A-Z]{2}\d{4}[A-Z]{2}")]
    public string? RegistrationNumber { get; set; }

    [Required]
    [XmlElement("VinNumber")]
    [MinLength(17)]
    [MaxLength(17)]
    public string VinNumber { get; set; } = null!;

    [Required]
    [XmlElement("TankCapacity")]
    [Range(950,1420)]
    public int TankCapacity { get; set; }

    [Required]
    [XmlElement("CargoCapacity")]
    [Range(5000,29000)]
    public int CargoCapacity { get; set; }

    [Required]
    [XmlElement("CategoryType")]
    public string CategoryType { get; set; }

    [Required]
    [XmlElement("MakeType")]
    public string MakeType { get; set; }
}

