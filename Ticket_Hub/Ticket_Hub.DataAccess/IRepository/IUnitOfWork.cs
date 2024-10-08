using Ticket_Hub.DataAccess.IRepository;

public interface IUnitOfWork
{
    IRefreshTokenRepository RefreshTokens { get; }
    Task<int> SaveAsync();
}