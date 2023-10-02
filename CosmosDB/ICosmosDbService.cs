using SBShared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosDB
{
    public interface ICosmosDbService
    {
        Task<List<PersonModel>> GetPersons();
    }
}
