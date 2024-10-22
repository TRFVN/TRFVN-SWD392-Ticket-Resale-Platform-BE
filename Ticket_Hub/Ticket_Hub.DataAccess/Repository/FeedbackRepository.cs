using Microsoft.EntityFrameworkCore;
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

        public void Update(Feedback feedback)
        {
            _context.Feedbacks.Update(feedback);
        }

        public void UpdateRange(IEnumerable<Feedback> feedbacks)
        {
            _context.Feedbacks.UpdateRange(feedbacks);
        }

        public async Task<Feedback> GetById(Guid feedbackId)
        {
            return await _context.Feedbacks.FirstOrDefaultAsync(x => x.FeedbackId == feedbackId);
        }
    }
}
