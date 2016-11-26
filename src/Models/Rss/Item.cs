namespace RssReader.Models.Rss
{
    using System.Xml.Serialization;

    public sealed class Item
    {
        [XmlElement("pubDate")]
        public string PubDate { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("guid")]
        public string Guid { get; set; }
    }
}
