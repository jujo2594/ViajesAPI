using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Transport : BaseEntity
    {
        [Required]
        public string FlightCarrier { get; set; }
        [Required]
        public string FlightNumber { get; set; }
        public ICollection<Flight> Flights { get; set; }
    }
}
