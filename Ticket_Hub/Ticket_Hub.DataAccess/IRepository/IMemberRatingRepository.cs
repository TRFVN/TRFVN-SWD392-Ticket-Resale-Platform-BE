﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository
{
    public interface IMemberRatingRepository : IRepository<MemberRating>
    {
        void Update(MemberRating memberRating);
        void UpdateRange(IEnumerable<MemberRating> memberRatings);
        Task<MemberRating> GetById(Guid memberRatingId);
    }
}
