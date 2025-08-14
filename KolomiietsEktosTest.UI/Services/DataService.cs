using KolomiietsEktosTest.UI.Models;
using MongoDB.Driver;

namespace KolomiietsEktosTest.UI.Services
{
    public class DataService
    {
        private readonly IMongoCollection<HeaderBytes> _itemsCollection;

        public DataService(IMongoDatabase database)
        {
            _itemsCollection = database.GetCollection<HeaderBytes>("Items");
        }

        public async Task<List<HeaderBytes>> GetAllItemsAsync()
        {
            return await _itemsCollection
                .Find(Builders<HeaderBytes>.Filter.Empty)
                .ToListAsync();
        }
    }
}
