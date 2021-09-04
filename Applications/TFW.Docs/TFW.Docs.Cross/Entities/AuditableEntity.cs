using TFW.Framework.Cross.Audit;

namespace TFW.Docs.Cross.Entities
{
    public interface IAppAuditableEntity : IAuditableEntity<int?>
    {
    }

    public interface IAppSoftDeleteEntity : ISoftDeleteEntity<int?>
    {
    }

    public abstract class AppAuditableEntity : AuditableEntity<int?>, IAppAuditableEntity
    {
    }

    public abstract class AppSoftDeleteEntity : SoftDeleteEntity<int?>, IAppSoftDeleteEntity
    {
    }

    public abstract class AppFullAuditableEntity : FullAuditableEntity<int?>,
        IAppAuditableEntity, IAppSoftDeleteEntity
    {
    }
}
