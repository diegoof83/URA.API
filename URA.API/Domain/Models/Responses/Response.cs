using System.Collections.Generic;

namespace URA.API.Domain.Models.Responses
{
    public class Response
    {       
        public bool Success { get; set; }
        public List<string> Erros { get; set; }
    }
}
