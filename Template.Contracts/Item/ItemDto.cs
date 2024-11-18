using System.Security.AccessControl;
using System.Xml.Serialization;

namespace Template.Contracts.Item
{
    public class ItemDto
    {
        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("itemName")]
        public string ItemName { get; set; }

        [XmlElement("price")]
        public string Price { get; set; }

        [XmlElement("quantity")]
        public int Quantity { get; set; }
    }
}
