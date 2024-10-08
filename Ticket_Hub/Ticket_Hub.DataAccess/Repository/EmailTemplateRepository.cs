using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository;

public class EmailTemplateRepository : Repository<EmailTemplate>, IEmailTemplateRepository
{
    private readonly ApplicationDbContext _context;

    public EmailTemplateRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(EmailTemplate emailTemplate)
    {
        _context.EmailTemplates.Update(emailTemplate);
    }
}