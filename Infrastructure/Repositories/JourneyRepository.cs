using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class JourneyRepository : GenericRepository<Journey>, IJourney
    {
        private readonly ViajesAPIContext _context;
        public JourneyRepository(ViajesAPIContext context) : base(context)
        {
            _context = context;
        }

        //--------------METODO QUE PERMITE OBTENER LAS ESCALAS ENTRE "origin" y "destination"
        public async Task<IEnumerable<Object>> GetStepsInTrip(string origin, string destination)
        {
            var steps = new List<object>();
            // Obtener los vuelos directos desde el origen hasta el destino
            var directFlights = await _context.Flights
                .Include(f => f.Transport)
                .Where(f => f.DepartureStation == origin && f.ArrivalStation == destination)
                .ToListAsync();
            // Si hay vuelos directos, agregarlos al viaje
            if (directFlights.Any())
            {
                var directStep = new
                {
                    Journey = new
                    {
                        Origin = origin.ToUpper(),
                        Destination = destination.ToUpper(),
                        Price = directFlights.Sum(f => f.Price),
                        Flights = directFlights.Select(f => new
                        {
                            DepartureStation = f.DepartureStation,
                            ArrivalStation = f.ArrivalStation,
                            Price = f.Price,
                            Transport = new
                            {
                                FlightCarrier = f.Transport.FlightCarrier,
                                FlightNumber = f.Transport.FlightNumber
                            }
                        })
                    }
                };
                steps.Add(directStep);
            }
            // Buscar vuelos de conexión para las escalas
            var connectingFlights = await _context.Flights
                .Include(f => f.Transport)
                .Where(f => f.DepartureStation == origin || f.ArrivalStation == destination)
                .ToListAsync();
            foreach (var connectingFlight in connectingFlights)
            {
                // Encontrar el segundo tramo del vuelo de conexión
                var secondLeg = await _context.Flights
                    .Include(f => f.Transport)
                    .FirstOrDefaultAsync(f => f.DepartureStation == connectingFlight.ArrivalStation && f.ArrivalStation == destination);
                if (secondLeg != null)
                {
                    var connectingStep = new
                    {
                        Journey = new
                        {
                            Origin = origin.ToUpper(),
                            Destination = destination.ToUpper(),
                            Price = connectingFlight.Price + secondLeg.Price,
                            Flights = new[]
                            {
                        new
                        {
                            DepartureStation = origin,
                            ArrivalStation = connectingFlight.ArrivalStation,
                            Price = connectingFlight.Price,
                            Transport = new
                            {
                                FlightCarrier = connectingFlight.Transport.FlightCarrier,
                                FlightNumber = connectingFlight.Transport.FlightNumber
                            }
                        },
                        new
                        {
                            DepartureStation = connectingFlight.ArrivalStation,
                            ArrivalStation = destination,
                            Price = secondLeg.Price,
                            Transport = new
                            {
                                FlightCarrier = secondLeg.Transport.FlightCarrier,
                                FlightNumber = secondLeg.Transport.FlightNumber
                            }
                        }
                    }
                        }
                    };
                    steps.Add(connectingStep);
                }
            }
            //await SaveQuery(steps);
            return steps;
        }

        // -------------------------------------- METODO QUE PERMITE OBTENER RESPUESTA DEL API 
        public async Task<JArray> GetJsonFromApi()
        {
            using (var client = new HttpClient())
            {
                string url = "https://bitecingcom.ipage.com/testapi/avanzado.js";
                client.DefaultRequestHeaders.Clear();
                var response = await client.GetAsync(url);
                var read = await response.Content.ReadAsStringAsync();
                dynamic answer = JArray.Parse(read);
                return answer;
            }
        }

        // ------------------------------------------------- MAPEO DEL API 
        public async Task<IEnumerable<Journey>> MapToJourneys()
        {
            var jsonArray = await GetJsonFromApi();
            var journeys = jsonArray.Select(j =>
                new Journey
                {
                    Origin = j["DepartureStation"].ToString(),
                    Destination = j["ArrivalStation"].ToString(),
                    Price = Convert.ToDouble(j["Price"]),
                    Flights = new List<Flight>
                    {
                        new Flight
                        {
                            DepartureStation = j["DepartureStation"].ToString(),
                            ArrivalStation = j["ArrivalStation"].ToString(),
                            Price = Convert.ToDouble(j["Price"]),
                            Transport = new Transport
                            {
                                FlightCarrier = j["FlightCarrier"].ToString(),
                                FlightNumber = j["FlightNumber"].ToString()
                            }
                        }
                    }
                }).ToList();
            var uniqueJourneys = new List<Journey>();
            foreach (var journey in journeys)
            {
                if (!_context.Journeys.Any(j =>
                    j.Origin == journey.Origin && j.Destination == journey.Destination))
                {
                    uniqueJourneys.Add(journey);
                }
            }
            return uniqueJourneys;
        }

        // ---------------------------------------- METODO QUE PERMITE GUARDAR EL MAPEO EN EL CONTEXTO DE MI APLICACION
    public async Task SaveMappInfo()
    {
            var journeys = await MapToJourneys();
            _context.Journeys.AddRange(journeys);
            await _context.SaveChangesAsync();
            foreach (var journey in journeys)
            {
                foreach (var flight in journey.Flights)
                {
                    flight.IdTransportFk = GetTransportId(flight.Transport.FlightCarrier, flight.Transport.FlightNumber);
                }
            }
            await _context.SaveChangesAsync();
    }

    private int GetTransportId (string flightCarrier, string flightNumber)
        {
            var transport = _context.Transports.FirstOrDefault(t => t.FlightCarrier == flightCarrier && t.FlightNumber == flightNumber);
            if (transport != null)
            {
                return transport.Id;
            }
            else
            {
                throw new Exception("No se encontro el vuelo asociado");
            }
        }
    }
}   
