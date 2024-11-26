using Sabio.Models;
using Sabio.Models.Domain.ProviderRating;
using Sabio.Models.Requests.ProviderRating;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sabio.Services.Interfaces
{
    public interface IUsersService
    {
        public void AddRating2Provider(ProviderRatingAddRequest model, int consumerId);
        public List<ProviderRating> GetProviderRatingsByProvId(int providerId);
        public List<ProviderRating> GetProviderRatings();
        public List<BasicUser> GetProviderUsers();
    }
}
