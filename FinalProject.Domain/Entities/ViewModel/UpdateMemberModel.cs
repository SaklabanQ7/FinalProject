using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.Entities.ViewModel
{
    public class UpdateMemberModel
    {
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]+$",
        ErrorMessage = "Password must be contain at least a letter and a number.")]
        public string Password { get; set; }
    }
}
