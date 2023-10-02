using Microsoft.Extensions.Options;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Collections.ObjectModel;
using SBShared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDB
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly CosmosDbSettings _settings;
        public CosmosDbService(IOptions<CosmosDbSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<List<PersonModel>> GetPersons()
        {
            var client = new CosmosClient(_settings.ConnectionString);
            var container = client.GetDatabase(_settings.DatabaseName).GetContainer(_settings.ContainerName);
            var queryable = container.GetItemLinqQueryable<PersonModel>(requestOptions: new QueryRequestOptions() { MaxItemCount = -1});// default 100, -1 for auto set
            var results = new List<PersonModel>();
            using FeedIterator<PersonModel> feed = queryable.OrderBy(x => x.FirstName).ToFeedIterator();
            while (feed.HasMoreResults)
            {
                var response = await feed.ReadNextAsync();
                foreach(PersonModel item in response)
                {
                    results.Add(item);
                }
            }
            return results;
        }
    }
}
