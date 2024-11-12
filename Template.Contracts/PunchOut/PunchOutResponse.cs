namespace Template.Contracts.PunchOut
{
    public class PunchOutResponse
    {
        public string ResponseMessage { get; }

        public PunchOutResponse(string statusCode, string message)
        {
            ResponseMessage = GeneratePunchOutResponseCxml(statusCode, message);
        }

        private static string GeneratePunchOutResponseCxml(string statusCode, string message)
        {
            // Sestavení cXML odpovědi pomocí StringBuilder
            var responseMessage = $@"
                <!DOCTYPE cXML SYSTEM ""http://xml.cXML.org/schemas/cXML/1.2.040/cXML.dtd"">
                <cXML payloadID=""{Guid.NewGuid()}@example.com"" timestamp=""{DateTime.UtcNow:yyyy-MM-ddTHH:mm:sszzz}"">
                  <Response>
                    <Status code=""{statusCode}"" text=""{message}""></Status>
                      <PunchOutSetupResponse>
                        <StartPage>
                          <URL>https://www.example.com/punchout?sid={Guid.NewGuid()}</URL>
                        </StartPage>
                      </PunchOutSetupResponse>
                  </Response>
                </cXML>";

            return responseMessage;
        }
    }
}
