using System.ComponentModel.DataAnnotations;

namespace ViajesAPI.Dto
{
    public class JourneyDto
    {
        public int Id { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }
    }
}
