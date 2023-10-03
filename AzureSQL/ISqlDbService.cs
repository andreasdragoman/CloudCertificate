using SBShared.Models;
using System.Collections.Generic;

namespace AzureSQL
{
    public interface ISqlDbService
    {
        public List<PostModel> GetAllPosts();
    }
}
