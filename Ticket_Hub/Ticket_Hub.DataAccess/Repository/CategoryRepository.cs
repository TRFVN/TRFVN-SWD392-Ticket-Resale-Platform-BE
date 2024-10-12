using Microsoft.EntityFrameworkCore;
using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Category category)
    {
        _context.Categories.Update(category);
    }

    public void UpdateRange(IEnumerable<Category> categories)
    {
        _context.Categories.UpdateRange(categories);
    }

    public async Task<Category> GetById(Guid categoryId)
    {
        return await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == categoryId);
    }
}