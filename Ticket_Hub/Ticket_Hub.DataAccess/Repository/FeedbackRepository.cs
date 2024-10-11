using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository
{
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
    {
        private readonly ApplicationDbContext _context;

        public FeedbackRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Feedback?> GetByIdAsync(Guid id)
        {
            return await _context.Feedbacks.FindAsync(id);
        }

        public void UpdateFeedback(Feedback feedback)
        {
            _context.Feedbacks.Update(feedback);
        }
    }
}
