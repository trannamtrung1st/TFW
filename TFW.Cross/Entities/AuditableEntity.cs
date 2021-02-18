using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Cross.Models;

namespace TFW.Cross.Entities
{
    public interface IAppAuditableEntity : IAuditableEntity<string>
    {
    }

    public interface IAppShallowDeleteEntity : IShallowDeleteEntity<string>
    {
    }

    public abstract class AppAuditableEntity : AuditableEntity<string>
    {
    }

    public abstract class AppShallowDeleteEntity : ShallowDeleteEntity<string>
    {
    }

    public abstract class AppFullAuditableEntity : FullAuditableEntity<string>
    {
    }
}
