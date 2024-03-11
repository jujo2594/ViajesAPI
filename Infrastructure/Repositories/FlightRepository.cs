using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Repositories
{
    public class FlightRepository : GenericRepository<Flight>, IFlight
    {
        private readonly ViajesAPIContext _context;

        public FlightRepository(ViajesAPIContext context) : base(context)
        {
            _context = context;
        }
    }
}


