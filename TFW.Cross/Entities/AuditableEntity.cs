using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Cross.Models;

namespace TFW.Cross.Entities
{
    public interface IAppAuditableEntity : IAuditableEntity<string>
    {
    }

    public interface IAppSoftDeleteEntity : ISoftDeleteEntity<string>
    {
    }

    public abstract class AppAuditableEntity : AuditableEntity<string>
    {
    }

    public abstract class AppSoftDeleteEntity : SoftDeleteEntity<string>
    {
    }

    public abstract class AppFullAuditableEntity : FullAuditableEntity<string>
    {
    }
}
