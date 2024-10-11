using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FeedbackService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateFeedbackAsync(Feedback feedback)
        {
            await _unitOfWork.Feedback.AddAsync(feedback);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteFeedbackAsync(Guid id)
        {
            var feedback = await _unitOfWork.Feedback.GetByIdAsync(id);
            if (feedback != null)
            {
                _unitOfWork.Feedback.Remove(feedback);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            return await _unitOfWork.Feedback.GetAllAsync();
        }

        public async Task<Feedback?> GetFeedbackByIdAsync(Guid id)
        {
            return await _unitOfWork.Feedback.GetByIdAsync(id);
        }

        public async Task UpdateFeedbackAsync(Feedback feedback)
        {
            _unitOfWork.Feedback.Update(feedback);
            await _unitOfWork.SaveAsync();
        }
    }
}
