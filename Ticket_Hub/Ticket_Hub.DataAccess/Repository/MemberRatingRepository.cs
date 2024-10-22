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
    public class MemberRatingRepository : Repository<MemberRating>, IMemberRatingRepository
    {
        private readonly ApplicationDbContext _context;

        public MemberRatingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(MemberRating memberRating)
        {
            _context.MemberRatings.Update(memberRating);
        }

        public void UpdateRange(IEnumerable<MemberRating> memberRatings)
        {
            _context.MemberRatings.UpdateRange(memberRatings);
        }

        public async Task<MemberRating> GetById(Guid memberRatingId)
        {
            return await _context.MemberRatings.FirstOrDefaultAsync(x => x.MemberRatingId == memberRatingId);
        }
    }
}
