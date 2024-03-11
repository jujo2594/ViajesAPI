using AutoMapper;
using Core.Entities;
using ViajesAPI.Dto;

namespace ViajesAPI.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Journey, JourneyDto>().ReverseMap();
            CreateMap<Flight, FlightDto>().ReverseMap();
            CreateMap<Transport, TransportDto>().ReverseMap();
        }
    }
}
