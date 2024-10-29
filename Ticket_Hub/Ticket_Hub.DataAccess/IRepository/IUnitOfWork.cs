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
    ICartHeaderRepository CartHeaderRepository { get; }
    ICartDetailRepository CartDetailRepository { get; }
    IChatRoomRepository ChatRoomRepository { get; }
    ICompanyRepository CompanyRepository { get; }
    IPrivacyRepository PrivacyRepository { get; }
    ITermOfUseRepository TermOfUseRepository { get; }
    IAppLogoRepository AppLogoRepository { get; }
    Task<int> SaveAsync();
}