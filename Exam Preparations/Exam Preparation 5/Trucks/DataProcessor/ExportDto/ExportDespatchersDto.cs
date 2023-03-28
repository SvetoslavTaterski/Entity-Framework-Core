using System.Xml.Serialization;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ExportDto;

[XmlType("Despatcher")]
public class ExportDespatchersDto
{
    [XmlAttribute("TrucksCount")]
    public int TrucksCount { get; set; }

    [XmlElement("DespatcherName")] 
    public string DespatcherName { get; set; } = null!;

    [XmlArray("Trucks")]
    public ExportTruckDto[] Trucks { get; set; }
}

[XmlType("Truck")]
public class ExportTruckDto
{
    [XmlElement("RegistrationNumber")]
    public string? RegistrationNumber { get; set; }

    [XmlElement("Make")]
    public string MakeType { get; set; } = null!;
}
