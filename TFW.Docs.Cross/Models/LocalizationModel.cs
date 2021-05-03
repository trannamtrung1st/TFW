using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Docs.Cross.Models
{
    public interface ILocalizationModel
    {
        public string Lang { get; set; }
        public string Region { get; set; }
    }

    public interface ICreateLocalizationModel : ILocalizationModel
    {
        public bool IsDefault { get; set; }
    }

    public interface ICreateLocalizedModel<T> where T : ICreateLocalizationModel
    {
        public IEnumerable<T> ListOfLocalization { get; set; }
    }
}
