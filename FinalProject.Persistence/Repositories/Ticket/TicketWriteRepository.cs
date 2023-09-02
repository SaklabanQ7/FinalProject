﻿using FinalProject.Domain.Entities;
using FinalProject.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Application.Repositories
{
    public class TicketWriteRepository : WriteRepository<Ticket>, ITicketWriteRepository
    {
        public TicketWriteRepository(OnlineEventsDbContext context) : base(context)
        {

        }
    
    }
}
