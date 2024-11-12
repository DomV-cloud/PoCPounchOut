using System.Xml.Linq;

namespace Template.Application.Services.MappingService
{
    public class CxmlMappingService : ICxmlMappingService
    {
        public string Parse(string xml)
        {
            var doc = XDocument.Parse(xml);

            // Access specific elements, e.g., "BuyerCookie" or "Identity" in "From"
            var buyerCookie = doc.Descendants("BuyerCookie").FirstOrDefault()?.Value;

            // simple validation
            if (buyerCookie is null)
            {
                return "";
            }

            return buyerCookie;
        }
    }
}
