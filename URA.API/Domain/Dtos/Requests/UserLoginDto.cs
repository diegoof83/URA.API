﻿using System.ComponentModel.DataAnnotations;

namespace URA.API.Domain.Dtos.Requests
{
    public class UserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}