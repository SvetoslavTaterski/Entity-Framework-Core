using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto;

[XmlType("Creator")]
public class ExportCreatorsDto
{
    [XmlAttribute("BoardgamesCount")]
    public int BoardgamesCount { get; set; }

    [XmlElement("CreatorName")]
    public string CreatorName { get; set; }

    [XmlArray("Boardgames")]
    public ExportBoardGamesDto[] Boardgames { get; set; }
}

[XmlType("Boardgame")]
public class ExportBoardGamesDto
{
    [XmlElement("BoardgameName")]
    public string BoardgameName { get; set; }

    [XmlElement("BoardgameYearPublished")]
    public int BoardgameYearPublished { get; set; }
}

