using Microsoft.AspNetCore.Identity;
using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository;

public class UnitOfWork: IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IRefreshTokenRepository RefreshTokens { get; set; }
    public IMemberRatingRepository MemberRating { get; set; }
    public IFavouriteRepository Favourite { get; private set; }
    public IFeedbackRepository Feedback { get; private set; }

    public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        RefreshTokens = new RefreshTokenRepository(_context);
        MemberRating = new MemberRatingRepository(_context);
        Favourite = new FavouriteRepository(_context);
        Feedback = new FeedbackRepository(_context);
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}