using System.Xml;
using System.Xml.Serialization;

namespace Integrations.EAuth
{
    public static class SamlSerialization
    {
        public static XmlSerializerNamespaces XmlNamespaces { get; set; }

        public static XmlDocument Serialize<T>(T item, XmlSerializerNamespaces namespaces = null)
        {
            var stream = new MemoryStream();
            Serialize(item, stream, namespaces);
            var document = new XmlDocument();
            stream.Seek(0L, SeekOrigin.Begin);
            document.Load(stream);
            stream.Close();
            return document;
        }

        public static void Serialize<T>(T item, Stream stream, XmlSerializerNamespaces namespaces = null)
        {
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, item, namespaces ?? XmlNamespaces);
            stream.Flush();
        }
    }
}
