using Template.Infrastructure.Interfaces;

namespace Template.Contracts.PunchOut
{
    public class SuccessPunchOutResponse : IPunchOutResponseGenerator
    {
        public string ResponseMessage { get; }

        public SuccessPunchOutResponse(string code, string text, string buyerCookie)
        {
            ResponseMessage = GeneratePunchOutResponse(code, text, buyerCookie: buyerCookie);
        }

        // should use DTO + add Payload ID!!!
        public string GeneratePunchOutResponse(string? code = "", string? text = "", string? message = "", string? buyerCookie = "")
        {
            // should you use StringBuilder 
            return $@"
                <!DOCTYPE cXML SYSTEM ""http://xml.cXML.org/schemas/cXML/1.2.040/cXML.dtd"">
                <cXML payloadID=""{Guid.NewGuid()}@example.com"" timestamp=""{DateTime.UtcNow:yyyy-MM-ddTHH:mm:sszzz}"">
                  <Response>
                    <Status code=""{code}"" text=""{text}""></Status>
                      <PunchOutSetupResponse>
                        <StartPage>
                          <URL>https://www.example.com/punchout?sid={buyerCookie}</URL>
                        </StartPage>
                      </PunchOutSetupResponse>
                  </Response>
                </cXML>";
        }
    }
}
