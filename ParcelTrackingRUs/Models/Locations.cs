using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ParcelTrackingRUs.Models
{
    public class Location
    {
        [BsonId]
        //public ObjectId Id { get; set; }
        public int Id { get; set; }

        public string? EntityId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
