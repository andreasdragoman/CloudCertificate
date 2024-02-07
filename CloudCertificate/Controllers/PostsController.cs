using AzureSQL;
using CloudCertificate.Services;
using Microsoft.AspNetCore.Mvc;
using SBShared.Models;

namespace CloudCertificate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController
    {
        private readonly IQueueService _queueService;
        private readonly ISqlDbService _sqlDbService;

        public PostsController(IQueueService queueService, ISqlDbService sqlDbService)
        {
            _queueService = queueService;
            _sqlDbService = sqlDbService;
        }

        [HttpGet]
        public List<PostModel> Get()
        {
            return _sqlDbService.GetAllPosts();
        }
    }
}
