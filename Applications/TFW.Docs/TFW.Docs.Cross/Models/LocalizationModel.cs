using System.Collections.Generic;

namespace TFW.Docs.Cross.Models
{
    public interface ILocalizationModel
    {
        public string Lang { get; set; }
        public string Region { get; set; }
    }

    public interface ICreateLocalizedModel<T> where T : ILocalizationModel
    {
        public IEnumerable<T> ListOfLocalization { get; set; }
    }
}
