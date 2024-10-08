using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Ticket_Hub.Models.Models
{
    public class RefreshTokens : BaseEntity<string, string, int>
    {
        [Key] public Guid RefreshTokensId { get; set; }
        [StringLength(450)] public string UserId { get; set; } = null!;
        [ForeignKey("UserId")] public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime Expires { get; set; }
    }
}
