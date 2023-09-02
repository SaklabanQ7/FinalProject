using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.Entities.ViewModel
{
    public class UpdateEventModel
    {
        public int Quota { get; set; }
        public string Address { get; set; }
        public int EventId { get; set; }

    }
}
