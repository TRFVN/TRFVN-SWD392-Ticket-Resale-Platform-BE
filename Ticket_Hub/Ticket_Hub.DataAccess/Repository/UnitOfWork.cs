using Microsoft.AspNetCore.Identity;
using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IRefreshTokenRepository RefreshTokens { get; set; }
    public IEmailTemplateRepository EmailTemplateRepository { get; set; }
    public ITicketRepository TicketRepository { get; set; }
    public IEventRepository EventRepository { get; set; }

    public ICategoryRepository CategoryRepository { get; set; }

    public IFeedbackRepository FeedbackRepository { get; set; }

    public IMessageRepository MessageRepository { get; set; }
    public IChatRoomRepository ChatRoomRepository { get; set; }


    public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        RefreshTokens = new RefreshTokenRepository(_context);
        EmailTemplateRepository = new EmailTemplateRepository(_context);
        TicketRepository = new TicketRepository(_context);
        EventRepository = new EventRepository(_context);
        CategoryRepository = new CategoryRepository(_context);

        FeedbackRepository = new FeedbackRepository(_context);

        MessageRepository = new MessageRepository(_context);

        FeedbackRepository = new FeedbackRepository(_context);

        MessageRepository = new MessageRepository(_context);    
        ChatRoomRepository = new ChatRoomRepository(_context);

    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}