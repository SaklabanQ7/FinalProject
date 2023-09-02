using FinalProject.Application.Repositories;
using FinalProject.Domain.Entities;
using FinalProject.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Persistence.Repositories
{
    public class AttendeeReadRepository : ReadRepository<Attendee>, IAttendeeReadRepository
    {
        public AttendeeReadRepository(OnlineEventsDbContext context) : base(context)
        {

        }
    }
}
