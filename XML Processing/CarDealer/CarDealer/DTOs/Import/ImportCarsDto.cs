using System.Xml.Serialization;

namespace CarDealer.DTOs.Import;

[XmlType("Car")]
public class ImportCarsDto
{
    [XmlElement("make")]
    public string Make { get; set; } = null!;

    [XmlElement("model")]
    public string Model { get; set; } = null!;

    [XmlElement("traveledDistance")]
    public long TraveledDistance { get; set; }

    [XmlArray("parts")]
    public PartIdDto[] Parts { get; set; }
}

[XmlType("partId")]
public class PartIdDto
{
    [XmlAttribute("id")]
    public int PartId { get; set; }
}

