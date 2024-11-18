namespace Template.Application.Services.MappingService
{
    public interface ICxmlMappingService
    {
        string ParseSecretId(string xml);

        string ParseBuyerCookie(string xml);

        string ParseUrl(string xml);
    }
}
