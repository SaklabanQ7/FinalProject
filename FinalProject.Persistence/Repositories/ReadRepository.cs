using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Application.Repositories;
using FinalProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Application.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : class 
    {
        private readonly OnlineEventsDbContext _context;
        public ReadRepository(OnlineEventsDbContext context)
        {
            _context = context;
        }
        public DbSet<T> Table => _context.Set<T>();
        public IQueryable<T> GetAll()
        {
            var query = Table.AsQueryable();
            
            return query;
        }
        
        public async Task<T> GetByIdAsync(int id)
        //=> await Table.FirstOrDefaultAsync(data =>data.Id==Guid.Parse(id));
        //=> await Table.FindAsync(Guid.Parse(id));
        {
            
            
            return await Table.FindAsync(id);
        }

    }
}
