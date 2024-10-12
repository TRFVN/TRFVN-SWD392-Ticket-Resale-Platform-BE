using Microsoft.EntityFrameworkCore;
using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository;

public class SubCategoryRepository : Repository<SubCategory>, ISubCategoryRepository
{
    private readonly ApplicationDbContext _context;
    
    public SubCategoryRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(SubCategory subCategory)
    {
        _context.SubCategories.Update(subCategory);
    }

    public void UpdateRange(IEnumerable<SubCategory> subCategories)
    {
        _context.SubCategories.UpdateRange(subCategories);
    }

    public async Task<SubCategory> GetById(Guid subCategoryId)
    {
        return await _context.SubCategories.FirstOrDefaultAsync(x => x.SubCategoryId == subCategoryId);
    }
}