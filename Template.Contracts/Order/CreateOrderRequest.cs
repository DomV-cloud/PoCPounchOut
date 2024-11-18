using System.Xml.Serialization;
using Template.Contracts.Item;

namespace Template.Contracts.Order
{
    [XmlRoot("CreateOrderRequest")]
    public class CreateOrderRequest
    {
        [XmlArray("products")]
        [XmlArrayItem("product")]
        public List<ItemDto> Products { get; set; } = [];

        [XmlElement("totalPrice")]
        public double TotalPrice { get; set; }

        [XmlElement("customerId")]
        public Guid CustomerId { get; set; }
    }
}
