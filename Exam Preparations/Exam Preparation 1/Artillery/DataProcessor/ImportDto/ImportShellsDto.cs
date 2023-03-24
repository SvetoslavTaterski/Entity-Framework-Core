using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Artillery.Common;

namespace Artillery.DataProcessor.ImportDto;

[XmlType("Shell")]
public class ImportShellsDto
{
    [Range(ValidationConstants.ShellMinWeight,ValidationConstants.ShellMaxWeight)]
    [XmlElement]
    public double ShellWeight { get; set; }

    [MinLength(ValidationConstants.CaliberMinLength)]
    [MaxLength(ValidationConstants.CaliberMaxLength)]
    [XmlElement]
    public string Caliber { get; set; } = null!;
}

