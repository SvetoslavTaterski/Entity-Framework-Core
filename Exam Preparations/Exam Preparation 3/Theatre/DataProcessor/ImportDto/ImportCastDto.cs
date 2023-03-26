using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Theatre.Common;

namespace Theatre.DataProcessor.ImportDto;

[XmlType("Cast")]
public class ImportCastDto
{
    [MaxLength(ValidationConstants.FullNameMaxLength)]
    [MinLength(ValidationConstants.FullNameMinLength)]
    [XmlElement("FullName")]
    public string FullName { get; set; } = null!;

    [XmlElement("IsMainCharacter")]
    public bool IsMainCharacter { get; set; }

    [XmlElement("PhoneNumber")]
    [RegularExpression(@"\+44-\d\d-\d\d\d-\d\d\d\d")]
    public string PhoneNumber { get; set; } = null!;

    [XmlElement("PlayId")]
    public int PlayId { get; set; }
}

