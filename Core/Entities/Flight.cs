using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Flight : BaseEntity
    {
        [Required]
        public string DepartureStation { get; set; }
        [Required]
        public string ArrivalStation { get; set; }
        [Required]
        public double Price{ get; set; }
        public int IdTransportFk { get; set; }
        public Transport Transport { get; set; }
        public ICollection<Journey> Journeys { get; set; } = new List<Journey>();
    }
}
