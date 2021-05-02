using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Cross.Models;

namespace TFW.Docs.Cross.Entities
{
    public abstract class AppLocalizedEntity<LEntity>
        : AppFullAuditableEntity, ILocalizedEntity<LEntity> where LEntity : ILocalizationEntity
    {
        public AppLocalizedEntity()
        {
            ListOfLocalization = new HashSet<LEntity>();
        }

        public virtual ICollection<LEntity> ListOfLocalization { get; set; }
    }

    public abstract class AppLocalizationEntity<EKey, TEntity>
        : AppAuditableEntity, ILocalizationEntity<EKey, TEntity> where TEntity : class
    {
        public string Lang { get; set; }
        public string Region { get; set; }
        public bool IsDefault { get; set; }
        public EKey EntityId { get; set; }
        public virtual TEntity Entity { get; set; }
    }
}
