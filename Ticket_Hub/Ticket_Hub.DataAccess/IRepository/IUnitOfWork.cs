using Ticket_Hub.DataAccess.IRepository;

public interface IUnitOfWork
{
    IRefreshTokenRepository RefreshTokens { get; }
    IEmailTemplateRepository EmailTemplateRepository { get; }
    ITicketRepository TicketRepository { get; }
    IEventRepository EventRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    ICartRepository CartRepository { get; }
    ICartItemRepository CartItemRepository { get; }
    IFeedbackRepository FeedbackRepository { get; }

    IMessageRepository MessageRepository { get; }
    IChatRoomRepository ChatRoomRepository { get; }
    IWalletRepository WalletRepository { get; }
    ITransactionRepository TransactionRepository { get; }
    INegotiationsRepository NegotiationsRepository { get; }
    Task<int> SaveAsync();
}