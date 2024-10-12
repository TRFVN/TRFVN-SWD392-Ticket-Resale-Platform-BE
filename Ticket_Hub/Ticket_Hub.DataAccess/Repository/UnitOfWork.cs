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
    public ILocationRepository LocationRepository { get; set; }
    public ICategoryRepository CategoryRepository { get; set; }
    public ISubCategoryRepository SubCategoryRepository { get; set; }

    public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        RefreshTokens = new RefreshTokenRepository(_context);
        EmailTemplateRepository = new EmailTemplateRepository(_context);
        TicketRepository = new TicketRepository(_context);
        //EventRepository = new EventRepository(_context);
        LocationRepository = new LocationRepository(_context);
        //CategoryRepository = new CategoryRepository(_context);
        //SubCategoryRepository = new SubCategoryRepository(_context);
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}