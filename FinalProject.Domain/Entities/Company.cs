using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace FinalProject.Domain.Entities
{
    public class Company 
    {
        [Key]
        public int CompanyId { get; set; }
        public decimal Commission { get; set; }

        public string CompanyName { get; set; }
        
        public string Website { get; set; }
        [EmailAddress]
        public string MailAddress { get; set; }
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]+$",
        ErrorMessage = "Password must be contain at least a letter and a number.")]
        public string Password { get; set; }
        public List<Ticket> Ticket { get; set; }




    }
}
