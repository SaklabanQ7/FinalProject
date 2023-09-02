using System.ComponentModel.DataAnnotations;

namespace FinalProject.Domain.Entities
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public int EventId { get; set; }

        public int CompanyId { get; set; }
        public decimal TicketPrice { get; set; }
        public Event Event { get; set; }
        public Company Company { get; set; }
    }
}
