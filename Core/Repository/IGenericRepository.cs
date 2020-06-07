using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Repository
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> GetAllAsync();
    }
}