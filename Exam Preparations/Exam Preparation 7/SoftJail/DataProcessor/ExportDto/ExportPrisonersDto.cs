using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto;

[XmlType("Prisoner")]
public class ExportPrisonersDto
{
    [XmlElement("Id")]
    public int Id { get; set; }

    [XmlElement("Name")]
    public string FullName { get; set; } = null!;

    [XmlElement("IncarcerationDate")]
    public string IncarcerationDate { get; set; } = null!;

    [XmlArray("EncryptedMessages")]
    public ExportMessageDto[] Message { get; set; }
}

[XmlType("Message")]
public class ExportMessageDto
{
    [XmlElement("Description")]
    public string Description { get; set; } = null!;
}

