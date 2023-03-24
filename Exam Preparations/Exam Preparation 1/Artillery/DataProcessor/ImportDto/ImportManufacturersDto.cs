using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Artillery.Common;

namespace Artillery.DataProcessor.ImportDto;

[XmlType("Manufacturer")]
public class ImportManufacturersDto
{
    [MaxLength(ValidationConstants.ManufacturerNameMaxLength)]
    [MinLength(ValidationConstants.ManufacturerNameMinLength)]
    [XmlElement("ManufacturerName")]
    public string ManufacturerName { get; set; } = null!;

    [MaxLength(ValidationConstants.FoundedMaxLength)]
    [MinLength(ValidationConstants.FoundedMinLength)]
    [XmlElement("Founded")]
    public string Founded { get; set; } = null!;
}

