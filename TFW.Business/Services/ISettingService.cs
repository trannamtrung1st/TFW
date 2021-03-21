using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TFW.Cross.Models.Setting;

namespace TFW.Business.Services
{
    public interface ISettingService
    {
        Task ChangeSmtpOptionAsync(ChangeSmtpOptionModel model);
        void ReloadConfiguration();
    }
}
