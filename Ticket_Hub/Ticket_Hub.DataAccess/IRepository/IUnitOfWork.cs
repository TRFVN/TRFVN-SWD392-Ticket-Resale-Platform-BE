using Ticket_Hub.DataAccess.IRepository;

public interface IUnitOfWork
{
    IMemberRatingRepository MemberRating { get; }
    IFavouriteRepository Favourite { get; }
    IFeedbackRepository Feedback { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    Task<int> SaveAsync();
}