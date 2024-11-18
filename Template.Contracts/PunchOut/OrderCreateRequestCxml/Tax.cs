using System.Xml.Serialization;

namespace Template.Contracts.PunchOut.OrderCreateRequestCxml
{
    public class Tax
    {
        [XmlElement("Money")]
        public Money Money { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }
    }
}
