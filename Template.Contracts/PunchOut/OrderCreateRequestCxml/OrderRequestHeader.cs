using System.Xml.Serialization;

namespace Template.Contracts.PunchOut.OrderCreateRequestCxml
{
    [XmlRoot("OrderRequestHeader")]
    public class OrderRequestHeader
    {
        [XmlAttribute("orderID")]
        public string OrderID { get; set; }

        [XmlAttribute("orderDate")]
        public string OrderDate { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("orderType")]
        public string OrderType { get; set; }

        [XmlElement("Total")]
        public Total Total { get; set; }

        [XmlElement("ShipTo")]
        public Address ShipTo { get; set; }

        [XmlElement("BillTo")]
        public Address BillTo { get; set; }

        [XmlElement("Shipping")]
        public Shipping Shipping { get; set; }

        [XmlElement("Tax")]
        public Tax Tax { get; set; }

        Guid CustomerId { get; set; }
    }
}
