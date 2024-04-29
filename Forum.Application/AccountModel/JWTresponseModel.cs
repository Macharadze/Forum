using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Application.AccountModel
{
    public class JWTresponseModel
    {
        public List<Claim> Claims { get; set; }

    }
}
