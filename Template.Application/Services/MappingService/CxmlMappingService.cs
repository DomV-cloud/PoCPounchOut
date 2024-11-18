using System.Xml;
using System.Xml.Linq;

namespace Template.Application.Services.MappingService
{
    public class CxmlMappingService : ICxmlMappingService
    {
        public string ParseBuyerCookie(string xml)
        {
            var doc = XDocument.Parse(xml);

            var buyerCookie = doc.Descendants("BuyerCookie").FirstOrDefault()?.Value;

            return buyerCookie ?? "";
        }

        public string ParseSecretId(string xml)
        {
            var doc = XDocument.Parse(xml);

            // Najdi Extrinsic s name="SecretId"
            var secretId = doc.Descendants("Extrinsic")
                .FirstOrDefault(e => (string)e.Attribute("name") == "SecretId")
                ?.Value;

            return secretId ?? "";
        }

        public string ParseUrl(string xml)
        {
            var doc = XDocument.Parse(xml);

            var url = doc.Descendants("BrowserFormPost").Elements("URL").FirstOrDefault()?.Value;

            return url ?? "";
        }
    }
}
