using MongoDB.Driver;
using ParcelTrackingRUs.Models;
using Microsoft.Extensions.Options;

namespace ParcelTrackingRUs.Services
{
    public class LocationService
    {
        private readonly IMongoCollection<Location> _locationsCollection;

        public LocationService(IOptions<MongoDbSettings> mongoSettings)
        {
            var mongoClient = new MongoClient(mongoSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);

            _locationsCollection = mongoDatabase.GetCollection<Location>("Locations");
        }

        public async Task CreateAsync(Location location) => await _locationsCollection.InsertOneAsync(location);

        public async Task<Location> GetLatestLocationAsync(string entityId) =>
            await _locationsCollection
                .Find(loc => loc.EntityId == entityId)
                .SortByDescending(loc => loc.Timestamp)
                .FirstOrDefaultAsync();
    }
}
