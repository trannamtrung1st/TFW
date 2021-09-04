using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TFW.Docs.Cross.Models.Identity
{
    public class RequestTokenModel //Resource Owner Password Credentials Grant
    {
        //REQUIRED. 
        //- password: grant username and password
        //- refresh_token: grant refresh_token
        [FromForm(Name = "grant_type")]
        public string GrantType { get; set; }

        //REQUIRED.  The resource owner username.
        [FromForm(Name = "username")]
        public string Username { get; set; }

        //REQUIRED.  The resource owner password.
        [FromForm(Name = "password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //OPTIONAL.  The refresh_token
        [FromForm(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        //OPTIONAL.  The scope of the access request as described by
        [FromForm(Name = "scope")]
        public string Scope { get; set; }
    }
}
