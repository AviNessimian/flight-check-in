using Britannica.Domain.Bases;
using System.Text.Json.Serialization;

namespace Britannica.Domain.Entities
{
    public class SeatEntity : AuditableEntity
    {
        public int Id { get; set; }

        [JsonIgnore]
        public int RowVersion { get; set; }

        public string Row { get; set; }
        public ushort Number { get; set; }

        public bool? IsAvailable { get; set; }

        public int AircraftRef { get; set; }

        [JsonIgnore]
        public AircraftEntity Flight { get; set; }
    }
}
