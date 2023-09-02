using FinalProject.Domain.Entities;
using FinalProject.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Application.Repositories
{
    public class MemberWriteRepository : WriteRepository<Member>, IMemberWriteRepository
    {
        public MemberWriteRepository(OnlineEventsDbContext context) : base(context)
        {

        }
    
    }
}
