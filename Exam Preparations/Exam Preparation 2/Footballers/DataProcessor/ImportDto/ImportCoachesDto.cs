using System.ComponentModel.DataAnnotations;
using Footballers.Data.Models;
using Footballers.Data.Models.Enums;
using System.Xml.Serialization;
using Castle.Components.DictionaryAdapter;
using Footballers.Common;

namespace Footballers.DataProcessor.ImportDto;

[XmlType("Coach")]
public class ImportCoachesDto
{
    [MaxLength(ValidationConstants.CoachNameMaxLength)]
    [MinLength(ValidationConstants.CoachNameMinLength)]
    [XmlElement("Name")]
    public string Name { get; set; } = null!;

    [XmlElement("Nationality")]
    public string Nationality { get; set; } = null!;

    [XmlArray("Footballers")]
    public ImportFootballersDto[] Footballers { get; set; }
}

[XmlType("Footballer")]
public class ImportFootballersDto
{
    [MaxLength(ValidationConstants.FootballerNameMaxLength)]
    [MinLength(ValidationConstants.FootballerNameMinLength)]
    [XmlElement("Name")]
    public string Name { get; set; } = null!;

    [XmlElement("ContractStartDate")]
    public string ContractStartDate { get; set; }

    [XmlElement("ContractEndDate")]
    public string ContractEndDate { get; set; }

    [XmlElement("BestSkillType")]
    public string BestSkillType { get; set; }

    [XmlElement("PositionType")]
    public string PositionType { get; set; }
}

