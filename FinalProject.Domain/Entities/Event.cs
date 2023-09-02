using System.ComponentModel.DataAnnotations;

namespace FinalProject.Domain.Entities
{
    public class Event 
    {
        public int MemberId { get; set; }
        [Key]
        public int EventId { get; set; }
        public string Name { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime ApplyDeadline { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string? Status { get; set; }

        public string Address { get; set; }
        public int Quota { get; set; } 
        public bool TicketInfo { get; set; }
        public string Category { get; set; }
        public decimal TicketPrice { get; set; }
        public Member Member { get; set; }
        public List<Attendee> Attendees { get; set; }

        
        




    }
}
