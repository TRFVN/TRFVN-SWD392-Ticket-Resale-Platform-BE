using Ticket_Hub.DataAccess.IRepository;

public interface IUnitOfWork
{
    IRefreshTokenRepository RefreshTokens { get; }
    IEmailTemplateRepository EmailTemplateRepository { get; }
    ITicketRepository TicketRepository { get; }
    IEventRepository EventRepository { get; }
    ILocationRepository LocationRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    ISubCategoryRepository SubCategoryRepository { get; }
    IMemberRatingRepository MemberRatingRepository { get; }
    IFeedbackRepository FeedbackRepository { get; }
    IFavouriteRepository FavoriteRepository { get; }
    IMessageRepository MessageRepository { get; }
    Task<int> SaveAsync();
}