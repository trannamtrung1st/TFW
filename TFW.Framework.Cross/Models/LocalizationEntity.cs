using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Cross.Models
{
    public interface ILocalizedEntity<LKey, LEntity> where LEntity : class, ILocalizationEntity
    {
        public LKey DefaultLocalizationId { get; set; }
        public LEntity DefaultLocalization { get; set; }
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
