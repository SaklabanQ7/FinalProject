using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace FinalProject.Domain.Entities
{
    public class Member 
    {
        [Key]
        public int MemberId { get; set; }
        [MaxLength(20,ErrorMessage = "Should be max 20 character")]
        public string Name { get; set; }
        [MaxLength(20, ErrorMessage = "Should be max 20 character")]

        public string SurName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "You have to enter a Mail addres.")]
        public string MailAddress { get; set; }
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]+$",
        ErrorMessage = "Password must be contain at least a letter and a number.")]
        public string Password { get; set; }
        public string? Title { get; set; }
        public  List<Event> Events { get; set; } 


    }
}
