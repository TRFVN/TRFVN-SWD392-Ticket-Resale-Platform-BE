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
    public IMemberRatingRepository MemberRatingRepository { get; set; }
    public IFeedbackRepository FeedbackRepository { get; set; }
    public IFavouriteRepository FavoriteRepository { get; set; }
    public IMessageRepository MessageRepository { get; set; }
    public ICartHeaderRepository CartHeaderRepository { get; set; }
    public ICartDetailRepository CartDetailRepository { get; set; }
    public IChatRoomRepository ChatRoomRepository { get; set; }
    public ICompanyRepository CompanyRepository { get; set; }
    public IPrivacyRepository PrivacyRepository { get; set; }
    public ITermOfUseRepository TermOfUseRepository { get; set; }
    public IAppLogoRepository AppLogoRepository { get; set; }

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
        MemberRatingRepository = new MemberRatingRepository(_context);
        FeedbackRepository = new FeedbackRepository(_context);
        FavoriteRepository = new FavouriteRepository(_context);
        MessageRepository = new MessageRepository(_context);
        CartHeaderRepository = new CartHeaderRepository(_context);
        CartDetailRepository = new CartDetailRepository(_context);
        MemberRatingRepository = new MemberRatingRepository(_context);
        FeedbackRepository = new FeedbackRepository(_context);
        FavoriteRepository = new FavouriteRepository(_context);
        MessageRepository = new MessageRepository(_context);    
        ChatRoomRepository = new ChatRoomRepository(_context);
        CompanyRepository = new CompanyRepository(_context);
        PrivacyRepository = new PrivacyRepository(_context);
        TermOfUseRepository = new TermOfUseRepository(_context);
        AppLogoRepository = new AppLogoRepository(_context);
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}