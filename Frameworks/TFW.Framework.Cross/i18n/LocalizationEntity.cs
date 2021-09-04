using System.Collections.Generic;

namespace TFW.Framework.Cross.i18n
{
    public interface ILocalizedEntity<LEntity> where LEntity : class, ILocalizationEntity
    {
        public ICollection<LEntity> ListOfLocalization { get; set; }
    }

    public interface ILocalizationEntity
    {
        public string Lang { get; set; }
        public string Region { get; set; }
    }

    public interface ILocalizationEntity<EKey, TEntity> : ILocalizationEntity where TEntity : class
    {
        public EKey EntityId { get; set; }
        public TEntity Entity { get; set; }
    }
}
