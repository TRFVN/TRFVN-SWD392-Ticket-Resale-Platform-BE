using Ticket_Hub.DataAccess.IRepository;

public interface IUnitOfWork
{
    IRefreshTokenRepository RefreshTokens { get; }
    IEmailTemplateRepository EmailTemplateRepository { get; }
    ITicketRepository TicketRepository { get; }
    IEventRepository EventRepository { get; }
    ICategoryRepository CategoryRepository { get; }

    IFeedbackRepository FeedbackRepository { get; }

    IMessageRepository MessageRepository { get; }
    IChatRoomRepository ChatRoomRepository { get; }

    Task<int> SaveAsync();
}