using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TFW.Framework.i18n;
using TFW.Framework.i18n.Extensions;

namespace TFW.Docs.Cross.Models.AppUser
{
    public class GetListAppUsersResponseModel : AppUserBaseModel
    {
        private DateTime _clientCreatedTime;
        [JsonProperty("createdTime")]
        public DateTime CreatedTime
        {
            get => _clientCreatedTime; set
            {
                _clientCreatedTime = value.ToTimeZoneFromUtc(Time.ThreadTimeZone);
            }
        }
    }
}
