using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TFW.Docs.Cross.Models.Identity
{
    public class RequestTokenModel //Resource Owner Password Credentials Grant
    {
        //REQUIRED. 
        //- password: grant username and password
        //- refresh_token: grant refresh_token
        public string grant_type { get; set; }

        //REQUIRED.  The resource owner username.
        public string username { get; set; }

        //REQUIRED.  The resource owner password.
        [DataType(DataType.Password)]
        public string password { get; set; }

        //OPTIONAL.  The refresh_token
        public string refresh_token { get; set; }

        //OPTIONAL.  The scope of the access request as described by
        public string scope { get; set; }
    }
}
