using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TFW.Docs.Cross.Models.Setting;

namespace TFW.Docs.Business.Services
{
    public interface ISettingService
    {
        Task<bool> GetInitStatusAsync();
        Task ChangeSmtpOptionAsync(ChangeSmtpOptionModel model);
        void ReloadConfiguration();
    }
}
