using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Services
{
    public interface IDbInitializer
    {
        Task InitAsync();
    }
}
