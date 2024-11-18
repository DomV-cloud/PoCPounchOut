using Template.Infrastructure.Interfaces;

namespace Template.Domain.Entities.Punchout
{
    public class ErrorPunchOutResponse : IPunchOutResponseGenerator
    {
        public string? Code { get; set; }

        public string? text { get; set; }

        public string? Message { get; set; }

        public ErrorPunchOutResponse(string? code, string? text, string? message)
        {
            Code = code;
            this.text = text;
            Message = message;
        }

        public string GeneratePunchOutResponse(string? code = "", string? text = "", string? message = "", string? buyerCookie = "")
        {
            return $@"
                <?xml version=""1.0"" encoding=""UTF-8""?>
                <!DOCTYPE cXML SYSTEM ""http://xml.cXML.org/schemas/cXML/1.2.040/cXML.dtd"">
                <cXML payloadID=""{Guid.NewGuid()}@example.com"" timestamp=""{DateTime.UtcNow:yyyy-MM-ddTHH:mm:sszzz}"">
                    <Response>
                        <Status code=""{code}"" text=""{text}"">
                            {(message)}
                        </Status>";
        }
    }
}
