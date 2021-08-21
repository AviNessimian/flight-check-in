using Britannica.Domain.Bases;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Britannica.Domain.Entities
{
    public class AircraftEntity : AuditableEntity
    {
        public int Id { get; set; }
        public decimal WeightLimit { get; set; }
        //public string WeightUnits { get; set; }
        public ushort BaggagesLimit { get; set; }
        public ICollection<SeatEntity> Seats { get; set; }


        public int FlightRef { get; set; }

        [JsonIgnore]
        public FlightEntity Flight { get; set; }

        public int SeatsCount => (Seats?.Count() ?? 0);
        public int PassengerBagsLimit => BaggagesLimit / SeatsCount;


        public bool GetAvailableSeat()
        {


            return true;
        }
    }

}
