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
    public class TransportRepository : GenericRepository<Transport>, ITransport
    {
        private readonly ViajesAPIContext _context;
        public TransportRepository(ViajesAPIContext context) : base(context)
        {
            _context = context;
        }

    }
}
