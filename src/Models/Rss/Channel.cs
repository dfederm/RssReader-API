namespace RssReader.Models.Rss
{
    using System.Xml.Serialization;

    public sealed class Channel
    {
        [XmlElement("lastBuildDate")]
        public string LastBuildDate { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("item")]
        public Item[] Item { get; set; }
    }
}
