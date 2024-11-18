using System.Xml.Serialization;

namespace Template.Contracts.PunchOut.OrderCreateRequestCxml
{
    public class Shipping
    {
        [XmlElement("Money")]
        public Money Money { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }
    }
}
