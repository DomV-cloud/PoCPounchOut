using System.Xml.Serialization;

namespace Template.Contracts.PunchOut.OrderCreateRequestCxml
{
    public class Address
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("PostalAddress")]
        public PostalAddress PostalAddress { get; set; }

        [XmlElement("Email")]
        public string Email { get; set; }

        [XmlElement("Phone")]
        public string Phone { get; set; }
    }
}
