using CloudCertificate.Configs;
using CloudCertificate.Services;
using Core;
using CosmosDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SBShared.Models;

namespace CloudCertificate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly IQueueService _queueService;
        private readonly ICosmosDbService _cosmosDbService;
        private AppSettings appSettings { get; set; }

        public PersonsController(IOptions<AppSettings> settings, IQueueService queueService, ICosmosDbService cosmosDbService)
        {
            appSettings = settings.Value;
            _queueService = queueService;
            _cosmosDbService = cosmosDbService;
        }

        [HttpGet]
        public async Task<IList<PersonModel>> Get()
        {
            //var dbService = new PersonsDbService();
            //return dbService.GetAllPersons();

            //var url = appSettings.AzureFunctions.BaseURL;
            //HttpClient client = new()
            //{
            //    BaseAddress = new Uri(url)
            //};

            //var response = await client.GetAsync(appSettings.AzureFunctions.GetPersons);
            //var responseMessage = await response.Content.ReadAsStringAsync();

            //return JsonConvert.DeserializeObject<List<string>>(responseMessage);

            return await _cosmosDbService.GetPersons();
        }

        [HttpPost]
        public async Task<IActionResult> Add(PersonModel person)
        {
            await _queueService.SendMessageAsync(person, "add-person-queue");
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, PersonModel person)
        {
            //await _queueService.SendMessageAsync(person, "update-person-queue");
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string personId)
        {
            await _queueService.SendMessageAsync(personId, "delete-person-queue");
            return Ok();
        }
    }
}
