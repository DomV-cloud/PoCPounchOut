using System.Xml.Serialization;

namespace Template.Contracts.PunchOut.OrderCreateRequestCxml
{
    public class Total
    {
        [XmlElement("Money")]
        public Money Money { get; set; }
    }

}
