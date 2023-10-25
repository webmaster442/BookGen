using System.Xml.Serialization;

using BookGen.Domain.Rss;

namespace BookGen.DomainServices
{
    /// <summary>
    /// Represents an RSS serializer
    /// </summary>
    public sealed class RssSerializer
    {
        private readonly XmlSerializer _serializer;

        /// <summary>
        /// Creates a new instance of serializer
        /// </summary>
        public RssSerializer()
        {
            _serializer = new XmlSerializer(typeof(RssFeed));
        }

        /// <summary>
        /// Deserialize an RSS feed from a stream
        /// </summary>
        /// <param name="stream">stream to deserialize from</param>
        /// <returns>The deserialized feed</returns>
        public RssFeed? Deserialize(Stream stream)
        {
            return (RssFeed?)_serializer.Deserialize(stream);
        }

        /// <summary>
        /// Deserialize an RSS feed from an xml string
        /// </summary>
        /// <param name="feed">xml string representing the feed</param>
        /// <returns>The deserialized feed</returns>
        public RssFeed? Deserialize(string feed)
        {
            using (var reader = new StringReader(feed))
            {
                return (RssFeed?)_serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Serialize an RSS class to a stream
        /// </summary>
        /// <param name="target">target stream</param>
        /// <param name="feed">feed object</param>
        public void Serialize(Stream target, RssFeed feed)
        {
            _serializer.Serialize(target, feed);
        }
    }
}
