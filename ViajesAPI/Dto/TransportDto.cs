using System.ComponentModel.DataAnnotations;

namespace ViajesAPI.Dto
{
    public class TransportDto
    {
        public int Id { get; set; }
        public string FlightCarrier { get; set; }
        public string FlightNumber { get; set; }
    }
}
