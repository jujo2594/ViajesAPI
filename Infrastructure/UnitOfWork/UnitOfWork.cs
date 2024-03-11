using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ViajesAPIContext _context;
        private IJourney _journeys;
        private IFlight _flights;
        private ITransport _transports;
        public UnitOfWork(ViajesAPIContext context)
        {
            _context = context;
        }

        public IJourney Journeys
        {
            get 
            { 
                if ( _journeys == null)
                {
                    _journeys = new JourneyRepository(_context);
                }
                return _journeys;
            }
        }
        public IFlight Flights
        {
            get
            {
                if (_flights == null)
                {
                    _flights = new FlightRepository(_context);
                }
                return _flights;
            }
        }
        public ITransport Transports
        {
            get
            {
                if(_transports == null)
                {
                    _transports = new TransportRepository(_context);
                }
                return _transports;
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
