namespace Ticket_Hub.Models.DTO.Website;

public class UpdateAppLogoDto
{
    public Guid AppLogoId { get; set; }
    public string[]? WebLogo { get; set; }
    public string[]? ChPlayLogo { get; set; }
    public string[]? AppStoreLogo { get; set; }
    public string[]? QrCodeLogo { get; set; }
}