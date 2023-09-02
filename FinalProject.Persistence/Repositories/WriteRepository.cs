using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Persistence.Contexts;

namespace FinalProject.Application.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : class
    {
        readonly private OnlineEventsDbContext _context;

        public WriteRepository(OnlineEventsDbContext context)
        {
            _context = context;
        }
        public DbSet<T> Table => _context.Set<T>();

        public async Task<bool> AddAsync(T model)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(model);
            return entityEntry.State == EntityState.Added;
        }

        
        public bool Remove(int id)
        {
            T model =Table.Find(id);
             Table.Remove(model);
            return true;
        }
        public bool Update(T model)
        {
            EntityEntry entityEntry = Table.Update(model);
            return entityEntry.State == EntityState.Modified;

        }
        public  int Save()
        {
            return _context.SaveChanges();
             
        }
    }
}
