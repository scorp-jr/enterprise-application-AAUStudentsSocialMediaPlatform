using System.Threading;
using System.Threading.Tasks;

namespace AAU.Connect.BuildingBlocks.Domain
{
    public interface IRepository<T> where T : IEntity
    {
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
    }
}
