using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using Template.Api.Filters;
using Template.Application.Common.Interfaces.Persistance;
using Template.Application.DatabaseContext;
using Template.Application.Services.Logging;
using Template.Contracts.Order;
using Template.Domain.Entities;

namespace Template.Api.Controllers
{
    [ApiController]
    [Route("order")]
    [ErrorHandlingFilter]
    [Consumes("application/xml")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly TemplateContext _context;
        private readonly ILoggingService _loggingService;

        public OrderController(
            IOrderRepository orderRepository, 
            TemplateContext context, 
            ILoggingService loggingService)
        {
            _orderRepository = orderRepository;
            _context = context;
            _loggingService = loggingService;
        }

        [HttpPost("create")]
        [Consumes("application/xml")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest orderDto)
        {
            if (orderDto is null)
            {
                await _loggingService.LogErrorAsync("Order request was not deliver");
                return BadRequest();
            }

            await _loggingService.LogInformationAsync("Order request was deliver {0}", orderDto.TotalPrice);

            var createdOrder = await _orderRepository.CreateOrder(orderDto);

            if (createdOrder is null)
            {
                await _loggingService.LogErrorAsync("Cannot create new order");
                return BadRequest();
            }

            await _loggingService.LogInformationAsync("Created new order {0}", createdOrder.Id);

            var customer = await _context.Users.FirstOrDefaultAsync(u => u.Id == orderDto.CustomerId);
            
            if (customer is null)
            {
                await _loggingService.LogErrorAsync("User was not found");
                return BadRequest();
            }

            await _loggingService.LogInformationAsync("Sent by customer: {0} {1}", customer.FirstName, customer.LastName);

            var responseXml = GenerateCXmlResponse(createdOrder, customer);

            if (responseXml is null)
            {
                await _loggingService.LogErrorAsync("Generate XML response has failed");
                return BadRequest();
            }

            await _loggingService.LogInformationAsync("XML response: {0}", responseXml);

            return Content(responseXml, "application/xml");
        }

        public XElement GenerateOrderXml(CreateOrderRequest orderRequest, string orderID)
        {
            // Formátování dat, zajištění že orderDate je správně ve formátu
            string orderDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

            // Vytvoření cXML struktury
            var cXml = new XElement("OrderRequestHeader",
                new XAttribute("orderID", orderID),
                new XAttribute("orderDate", orderDate),
                new XAttribute("type", "new"),
                new XAttribute("orderType", "regular"),
                new XElement("Total",
                    new XElement("Money", new XAttribute("currency", "USD"),
                        orderRequest.TotalPrice.ToString("F2"))
                ),
                GenerateShippingElement(),
                GenerateTaxElement(),
                GenerateAddressElement("ShipTo", orderRequest.CustomerId), // Vytvoření ShipTo elementu
                GenerateAddressElement("BillTo", orderRequest.CustomerId)  // Vytvoření BillTo elementu
            );

            return cXml;
        }

        private static XElement GenerateShippingElement()
        {
            // Zde vypočítáváte cenu za dopravu nebo používáte nějakou konstantní hodnotu
            decimal shippingCost = 15.78m; // Můžete upravit podle potřeby
            return new XElement("Shipping",
                new XElement("Money", new XAttribute("currency", "USD"), shippingCost.ToString("F2")),
                new XElement("Description", new XAttribute("xml:lang", "en"), "Freight")
            );
        }

        private static XElement GenerateTaxElement()
        {
            // Zde vypočítáváte nebo použijete hodnotu daně
            decimal taxAmount = 5.12m; // Můžete upravit podle potřeby
            return new XElement("Tax",
                new XElement("Money", new XAttribute("currency", "USD"), taxAmount.ToString("F2")),
                new XElement("Description", new XAttribute("xml:lang", "en"), "CA Sales Tax (9%)")
            );
        }

        private static XElement GenerateAddressElement(string type, Guid customerId)
        {
            // Vygenerujte ShipTo nebo BillTo dle potřeby. Zde použijte údaje z databáze nebo další logiku.
            // Pro tento příklad jsou používány fiktivní hodnoty:

            return new XElement(type,
                new XElement("Address", new XAttribute("addressID", customerId.ToString()),
                    new XElement("Name", new XAttribute(XNamespace.Xml + "lang", "en"), "John Doe"),
                    new XElement("PostalAddress",
                        new XElement("DeliverTo", "John Doe"),
                        new XElement("Street", "123 Main Street"),
                        new XElement("City", "Some City"),
                        new XElement("State", "CA"),
                        new XElement("PostalCode", "12345"),
                        new XElement("Country", new XAttribute("isoCountryCode", "US"), "United States")
                    ),
                    new XElement("Email", "john.doe@example.com"),
                    new XElement("Phone",
                        new XElement("TelephoneNumber",
                            new XElement("CountryCode", new XAttribute("isoCountryCode", "US"), "1"),
                            new XElement("AreaOrCityCode", "555"),
                            new XElement("Number", "123456789")
                        )
                    )
                )
            );
        }

        private static string GenerateCXmlResponse(Order order, User customer)
        {
            var payloadId = $"{DateTime.UtcNow.Ticks}@mycompany.com";
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:sszzz");

            var cXml = new XElement("cXML",
        new XAttribute("payloadID", payloadId),
        new XAttribute("timestamp", timestamp),
        new XElement("Header",
            new XElement("From",
                new XElement("Credential", new XAttribute("domain", "NetworkId"),
                    new XElement("Identity", "test")
                )
            ),
            new XElement("To",
                new XElement("Credential", new XAttribute("domain", "DUNS"),
                    new XElement("Identity", "test")
                )
            ),
            new XElement("Sender",
                new XElement("Credential", new XAttribute("domain", "NetworkId"),
                    new XElement("Identity", "test"),
                    new XElement("SharedSecret", "123456789")
                ),
                new XElement("UserAgent", "cXML Tester (+https://punchoutcommerce.com/)")
            )
        ),
        new XElement("Request", new XAttribute("deploymentMode", "test"),
            new XElement("OrderRequest",
                new XElement("OrderRequestHeader",
                    new XAttribute("orderID", order.OrderNumber),
                    new XAttribute("orderDate", order.OrderDate.ToString("yyyy-MM-dd")),
                    new XAttribute("type", "new"),
                    new XAttribute("orderType", "regular"),
                    new XElement("Total",
                        new XElement("Money", new XAttribute("currency", "USD"),
                            order.OrderItems.Sum(item => item.ItemPrice * item.Quantity)
                        )
                    ),
                    GenerateAddressElement("ShipTo", customer),  // Nahraďte podle vašich dat
                    GenerateAddressElement("BillTo", customer)   // Nahraďte podle vašich dat
                ),
                order.OrderItems.Select((item, index) =>
                    new XElement("ItemOut",
                        new XAttribute("quantity", item.Quantity),
                        new XAttribute("lineNumber", index + 1),
                        new XElement("ItemID",
                            new XElement("SupplierPartID", item.Item.Id),
                            new XElement("SupplierPartAuxiliaryID", "AuxiliaryID") // Nahraďte podle vašich dat
                        ),
                        new XElement("ItemDetail",
                            new XElement("UnitPrice",
                                new XElement("Money", new XAttribute("currency", "USD"), item.ItemPrice)
                            ),
                            new XElement(XName.Get("Description", "xml"), "en", item.Item.ItemName),  // Opravený atribut
                            new XElement("UnitOfMeasure", "Unit"),  // Nahraďte podle vašich dat
                            new XElement("Classification", new XAttribute("domain", "UNSPSC"), "Classification"),  // Nahraďte podle vašich dat
                            new XElement("ManufacturerPartID", "ManufacturerID"),  // Nahraďte podle vašich dat
                            new XElement("ManufacturerName", "ManufacturerName")  // Nahraďte podle vašich dat
                        )
                    )
                )
            )
        )
    );

            return cXml.ToString();
        }

        private static XElement GenerateAddressElement(string type, User customer)
        {
            XNamespace xmlNamespace = "http://www.w3.org/XML/1998/namespace"; // Definice jmenného prostoru pro xml:lang

            return new XElement(type,
                new XElement("Address", new XAttribute("addressID", customer.Id.ToString()),
                    new XElement("Name", new XAttribute(XNamespace.Xml + "lang", "en"), customer.FirstName + " " + customer.LastName), // Použití správného jmenného prostoru pro xml:lang
                    new XElement("PostalAddress",
                        new XElement("DeliverTo", customer.FirstName),
                        new XElement("DeliverTo", customer.LastName),
                        new XElement("Street", "Customer Street"), // Nahraďte podle vašich dat
                        new XElement("City", "Customer City"),     // Nahraďte podle vašich dat
                        new XElement("State", "Customer State"),   // Nahraďte podle vašich dat
                        new XElement("PostalCode", "Customer ZIP"), // Nahraďte podle vašich dat
                        new XElement("Country", new XAttribute("isoCountryCode", "US"), "United States")
                    ),
                    new XElement("Email", customer.Email),
                    new XElement("Phone",
                        new XElement("TelephoneNumber",
                            new XElement("CountryCode", new XAttribute("isoCountryCode", "US"), "1"),
                            new XElement("AreaOrCityCode", "555"),
                            new XElement("Number", "1234567")
                        )
                    )
                )
            );
        }

    }
}
