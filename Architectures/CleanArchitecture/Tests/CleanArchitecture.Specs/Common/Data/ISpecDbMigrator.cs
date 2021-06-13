using Persistence.Services;
using System.Threading.Tasks;

namespace CleanArchitecture.Specs.Common.Data
{
    public interface ISpecDbMigrator : IDbMigrator
    {
        Task InitAsync(string dataSetKey);
    }
}
