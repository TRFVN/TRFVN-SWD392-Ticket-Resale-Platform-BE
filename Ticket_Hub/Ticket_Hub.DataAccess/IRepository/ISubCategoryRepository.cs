using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository;

public interface ISubCategoryRepository : IRepository<SubCategory>
{
    void Update(SubCategory subCategory);
    void UpdateRange(IEnumerable<SubCategory> subCategories);
    Task<SubCategory> GetById(Guid subCateogryId);
}