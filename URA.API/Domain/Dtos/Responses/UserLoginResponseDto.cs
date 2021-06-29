using System.Collections.Generic;

namespace URA.API.Domain.Dtos.Responses
{
    public class UserLoginResponseDto : ResponseDto
    {
        public string Token { get; set; }

        public List<string> Errors { get; set; }
    }
}
