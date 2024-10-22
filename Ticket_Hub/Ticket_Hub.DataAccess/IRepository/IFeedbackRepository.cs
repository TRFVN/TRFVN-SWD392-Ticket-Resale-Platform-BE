using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository
{
    public interface IFeedbackRepository : IRepository<Feedback>
    {
        void Update(Feedback feedback);
        void UpdateRange(IEnumerable<Feedback> feedbacks);
        Task<Feedback> GetById(Guid feedbackId);
    }
}
