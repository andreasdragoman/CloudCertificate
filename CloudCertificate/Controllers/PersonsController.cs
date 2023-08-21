using CloudCertificate.Configs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CloudCertificate.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PersonsController : ControllerBase
    {
        private AppSettings appSettings { get; set; }

        public PersonsController(IOptions<AppSettings> settings)
        {
            appSettings = settings.Value;
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
    }
}
