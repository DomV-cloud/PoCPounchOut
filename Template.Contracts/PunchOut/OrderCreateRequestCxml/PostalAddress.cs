using System.Xml.Serialization;

namespace Template.Contracts.PunchOut.OrderCreateRequestCxml
{
    public class PostalAddress
    {
        [XmlElement("DeliverTo")]
        public string DeliverTo { get; set; }

        [XmlElement("Street")]
        public string Street { get; set; }

        [XmlElement("City")]
        public string City { get; set; }

        [XmlElement("State")]
        public string State { get; set; }

        [XmlElement("PostalCode")]
        public string PostalCode { get; set; }

        [XmlElement("Country")]
        public string Country { get; set; }
    }
}
