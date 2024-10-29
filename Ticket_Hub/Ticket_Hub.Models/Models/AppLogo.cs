namespace Ticket_Hub.Models.Models;

public class AppLogo
{
    public Guid AppLogoId { get; set; }
    public string[]? WebLogo { get; set; }
    public string[]? ChPlayLogo { get; set; }
    public string[]? AppStoreLogo { get; set; }
    public string[]? QrCodeLogo { get; set; }
}