using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository;

public interface ICategoryRepository : IRepository<Category>
{
    void Update(Category category);
    void UpdateRange(IEnumerable<Category> categories);
    Task<Category> GetById(Guid categoryId);
}