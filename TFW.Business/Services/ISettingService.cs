using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Models.Setting;

namespace TFW.Business.Services
{
    public interface ISettingService
    {
        void ChangeSmtpOption(ChangeSmtpOptionModel model);
        void ReloadConfiguration();
    }
}
