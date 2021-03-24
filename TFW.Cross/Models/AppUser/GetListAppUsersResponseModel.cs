﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TFW.Framework.i18n;
using TFW.Framework.i18n.Helpers;

namespace TFW.Cross.Models.AppUser
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
