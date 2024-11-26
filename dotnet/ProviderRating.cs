using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.ProviderRating
{
    public class ProviderRating
    {
        public BasicUser Provider {  get; set; }
        public List<Ratings> Ratings { get; set; }

    }
}
