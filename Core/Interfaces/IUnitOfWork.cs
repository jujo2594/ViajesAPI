using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        IJourney Journeys { get; }
        IFlight Flights { get; }
        ITransport Transports { get; }
        Task<int> SaveAsync();
    }
}
