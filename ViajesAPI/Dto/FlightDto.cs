using System.ComponentModel.DataAnnotations;

namespace ViajesAPI.Dto
{
    public class FlightDto
    {
        public int Id { get; set; }
        public string DepartureStation { get; set; }
        public string ArrivalStation { get; set; }
        public double Price { get; set; }
    }
}
