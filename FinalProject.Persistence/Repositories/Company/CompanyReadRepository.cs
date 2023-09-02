using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Application.Repositories;
using FinalProject.Domain.Entities;
using FinalProject.Persistence.Contexts;

namespace FinalProject.Application.Repositories
{
    public class CompanyReadRepository : ReadRepository<Company>, ICompanyReadRepository
    {
        public CompanyReadRepository(OnlineEventsDbContext context) : base(context)
        {

        }
    }
}
