using System.Collections.Generic;

namespace URA.API.Domain.Dtos.Responses
{
    public class ResponseDto
    {       
        public bool Success { get; set; }
        public List<string> Erros { get; set; }
    }
}
