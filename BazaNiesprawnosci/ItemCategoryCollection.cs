using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System;

namespace BazaNiesprawnosci
{
    [XmlRoot("Collection")]
    [Serializable]
    public class ItemCategoryCollection: 
        ObservableCollection<TItemCategory>, IXmlSerializable
    {
        
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            XDocument doc = null;
            using (XmlReader subtreeReader = reader.ReadSubtree())
            {
                doc = XDocument.Load(subtreeReader);
            }
            XmlSerializer serializer = new XmlSerializer(typeof(TItemCategory));
            foreach (XElement item in doc.Descendants(XName.Get("Category")))
            {
                using (XmlReader itemReader = item.CreateReader())
                {
                    var cat = (TItemCategory)serializer.Deserialize(itemReader);
                    this.Add(cat);
                }
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TItemCategory));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            foreach (TItemCategory cat in this)
            {
                serializer.Serialize(writer, cat, ns);
            }
        }
    }
}