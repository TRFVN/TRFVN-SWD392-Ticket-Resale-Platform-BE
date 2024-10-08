namespace Ticket_Hub.DataAccess.IRepository;

public interface IUnitOfWork
{
    Task<int> SaveAsync();
}