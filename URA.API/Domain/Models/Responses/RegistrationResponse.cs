using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URA.API.Config;

namespace URA.API.Domain.Models.Responses
{
    public class RegistrationResponse : Response
    {
        public string Token { get; set; }
    }
}
