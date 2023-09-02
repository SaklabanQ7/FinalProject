using FinalProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace FinalProject.Persistence.Contexts
{
    public class OnlineEventsDbContext : DbContext
    {
        public OnlineEventsDbContext(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=OnlineEventDb;Trusted_Connection=True;Encrypt=False");


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Attendee>().HasKey(a => a.Id);
            modelBuilder.Entity<Attendee>().HasMany(a => a.Events);
            modelBuilder.Entity<Attendee>().HasOne(a => a.Member);
            modelBuilder.Entity<Member>().HasKey(a => a.MemberId);
            modelBuilder.Entity<Event>().HasKey(a => a.EventId);
            modelBuilder.Entity<Company>().HasKey(a => a.CompanyId);
            modelBuilder.Entity<Ticket>().HasKey(a => a.TicketId);
            modelBuilder.Entity<Member>().HasMany(a=>a.Events);
            modelBuilder.Entity<Event>().HasOne(a => a.Member);
            modelBuilder.Entity<Company>().HasMany(a => a.Ticket);
            modelBuilder.Entity<Ticket>().HasOne(a => a.Event);
            modelBuilder.Entity<Ticket>().HasOne(a => a.Company);
            modelBuilder.Entity<Member>().HasIndex(a => a.MailAddress).IsUnique();
            modelBuilder.Entity<Company>().HasIndex(a => a.MailAddress).IsUnique();
            modelBuilder.Entity<Member>().HasData(new Member
            {
                MemberId = 1903,
                Name="Onur",
                SurName="Kose",
                MailAddress = "onur1903kose@outlook.com",
                Password = "SaklabanQ7",
                Title = "Admin",
            });
        }
        
        public DbSet<Company> Companies { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
    }
}
