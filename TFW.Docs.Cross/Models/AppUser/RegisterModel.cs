using System.ComponentModel.DataAnnotations;

namespace TFW.Docs.Cross.Models.AppUser
{
    public class RegisterModel
    {
        public string username { get; set; }
        public string email { get; set; }

        [DataType(DataType.Password)]
        public string password { get; set; }

        [DataType(DataType.Password)]
        public string confirmPassword { get; set; }

        public string fullName { get; set; }
    }
}
