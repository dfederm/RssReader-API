namespace RssReader.Models.Rss
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "rss")]
    public sealed class Rss
    {
        [XmlElement("channel")]
        public Channel Channel { get; set; }
    }
}
