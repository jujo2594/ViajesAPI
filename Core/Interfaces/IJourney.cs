using Core.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace Core.Interfaces
    {
        public interface IJourney : IGenericRepository<Journey>
        {
            Task<JArray> GetJsonFromApi();
            Task<IEnumerable<Journey>> MapToJourneys();
            Task SaveMappInfo();
            Task<IEnumerable<Object>> GetStepsInTrip(string origin, string destination);
        }
    }
