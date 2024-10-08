using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository;

public interface IEmailTemplateRepository : IRepository<EmailTemplate>
{
    void Update(EmailTemplate emailTemplate);
}