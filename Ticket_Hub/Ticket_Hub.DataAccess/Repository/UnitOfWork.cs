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
<<<<<<< Updated upstream
=======
    public IMemberRatingRepository MemberRatingRepository { get; set; }
    public IFeedbackRepository FeedbackRepository { get; set; }
    public IFavouriteRepository FavoriteRepository { get; set; }
    public IMessageRepository MessageRepository { get; set; }
    public ICartHeaderRepository CartHeaderRepository { get; set; }
    public ICartDetailRepository CartDetailRepository { get; set; }
>>>>>>> Stashed changes

    public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        RefreshTokens = new RefreshTokenRepository(_context);
        EmailTemplateRepository = new EmailTemplateRepository(_context);
        TicketRepository = new TicketRepository(_context);
        EventRepository = new EventRepository(_context);
        LocationRepository = new LocationRepository(_context);
        CategoryRepository = new CategoryRepository(_context);
        SubCategoryRepository = new SubCategoryRepository(_context);
<<<<<<< Updated upstream
=======
        MemberRatingRepository = new MemberRatingRepository(_context);
        FeedbackRepository = new FeedbackRepository(_context);
        FavoriteRepository = new FavouriteRepository(_context);
        MessageRepository = new MessageRepository(_context);
        CartHeaderRepository = new CartHeaderRepository(_context);
        CartDetailRepository = new CartDetailRepository(_context);
>>>>>>> Stashed changes
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}