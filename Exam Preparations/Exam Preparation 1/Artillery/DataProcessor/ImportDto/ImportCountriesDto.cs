using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Artillery.Common;

namespace Artillery.DataProcessor.ImportDto;

[XmlType("Country")]
public class ImportCountriesDto
{
    [MaxLength(ValidationConstants.CountryNameMaxLength)]
    [MinLength(ValidationConstants.CountryNameMinLength)]
    [XmlElement("CountryName")]
    public string CountryName { get; set; } = null!;

    [Range(ValidationConstants.ArmyMinSize,ValidationConstants.ArmyMaxSize)]
    [XmlElement("ArmySize")]
    public int ArmySize { get; set; }
}

