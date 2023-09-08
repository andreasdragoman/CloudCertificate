using CloudCertificate.Configs;
using CloudCertificate.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SBShared.Models;

namespace CloudCertificate.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PersonsController : ControllerBase
    {
        private readonly IQueueService _queueService;
        private AppSettings appSettings { get; set; }

        public PersonsController(IOptions<AppSettings> settings, IQueueService queueService)
        {
            appSettings = settings.Value;
            _queueService = queueService;
        }

        [HttpGet]
        public async Task<IList<string>> Get()
        {
            var url = appSettings.AzureFunctions.BaseURL;
            HttpClient client = new()
            {
                BaseAddress = new Uri(url)
            };

            var response = await client.GetAsync(appSettings.AzureFunctions.GetPersons);
            var responseMessage = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<string>>(responseMessage);
        }

        [HttpPost]
        public async Task<IActionResult> Add(PersonModel person)
        {
            await _queueService.SendMessageAsync(person, "personqueue");
            return Ok();
        }
    }
}
