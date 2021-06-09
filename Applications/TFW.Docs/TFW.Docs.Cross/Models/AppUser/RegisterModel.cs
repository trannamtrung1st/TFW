using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TFW.Docs.Cross.Models.AppUser
{
    public class RegisterModel
    {
        [FromForm(Name = "username")]
        public string Username { get; set; }

        [FromForm(Name = "email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [FromForm(Name = "password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [FromForm(Name = "confirmPassword")]
        public string ConfirmPassword { get; set; }

        [FromForm(Name = "fullName")]
        public string FullName { get; set; }
    }
}
