using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using Actio.Api.Models;

namespace Actio.Api.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly IMongoDatabase _database;
        public ActivityRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync(Activity activity)
            => await Collection.InsertOneAsync(activity);

        public async Task<IEnumerable<Activity>> BrowseAsync(Guid userId)
            => await Collection
                .AsQueryable()
                .Where(x => x.UserId == userId)
                .ToListAsync();

        public async Task<Activity> GetAsync(Guid id)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == id); // <= MongoDB.Driver.Linq

        private IMongoCollection<Activity> Collection
            => _database.GetCollection<Activity>("Activities");
    }
}
