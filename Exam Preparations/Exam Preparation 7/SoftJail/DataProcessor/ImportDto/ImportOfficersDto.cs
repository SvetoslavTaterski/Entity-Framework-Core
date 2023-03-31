using SoftJail.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto;

[XmlType("Officer")]
public class ImportOfficersDto
{
    [Required]
    [XmlElement("Name")]
    [MinLength(3)]
    [MaxLength(30)]
    public string FullName { get; set; } = null!;

    [Required]
    [XmlElement("Money")]
    [Range(0, double.PositiveInfinity)]
    public decimal Salary { get; set; }

    [Required]
    [XmlElement("Position")]
    public string Position { get; set; } = null!;

    [Required]
    [XmlElement("Weapon")]
    public string Weapon { get; set; } = null!;

    [Required]
    [XmlElement("DepartmentId")]
    public int DepartmentId { get; set; }

    [XmlArray("Prisoners")]
    public ImportPrisonersOfficersDto[] Prisoners { get; set; }
}

[XmlType("Prisoner")]
public class ImportPrisonersOfficersDto
{
    [Required]
    [XmlAttribute("id")]
    public int Id { get; set; }
}

