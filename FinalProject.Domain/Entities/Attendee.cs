using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.Entities
{
    public class Attendee
    {
        [Key]
        public int Id { get; set; }
        public int MemberId { get; set; }

        public int EventId { get; set; }
        [MaxLength(20, ErrorMessage = "Should be max 20 character")]
        public string Name { get; set; }
        [MaxLength(20, ErrorMessage = "Should be max 20 character")]

        public string SurName { get; set; }
        public Member Member { get; set; }
        public List<Event> Events { get; set; }


    }
}
