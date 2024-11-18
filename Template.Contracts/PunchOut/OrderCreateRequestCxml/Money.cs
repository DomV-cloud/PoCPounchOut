using System.Xml.Serialization;

namespace Template.Contracts.PunchOut.OrderCreateRequestCxml
{
    public class Money
    {
        [XmlAttribute("currency")]
        public string Currency { get; set; }

        [XmlText]
        public decimal Value { get; set; }
    }
}
