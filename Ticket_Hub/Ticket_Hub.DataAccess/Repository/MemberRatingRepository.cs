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
        private readonly ApplicationDbContext _db;

        public MemberRatingRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task CreateRatingAsync(MemberRating rating)
        {
            await AddAsync(rating);
        }

        public async Task<MemberRating> GetByIdAsync(Guid id)
        {
            return await _db.MemberRatings.FirstOrDefaultAsync(m => m.MemberRatingId == id);
        }

        public void UpdateRating(MemberRating memberRating)
        {
            _db.MemberRatings.Update(memberRating);
        }

        public async Task RemoveRatingAsync(MemberRating rating)
        {
            Remove(rating);
        }
    }
}
