using System;
using System.Text;
using TFW.Cross.Models.Common;

namespace TFW.Cross.Models.AppUser
{
    public class GetListAppUsersRequestModel : BaseGetListRequestModel
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string searchTerm { get; set; }
    }
}
