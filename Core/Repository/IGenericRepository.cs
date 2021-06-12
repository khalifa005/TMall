using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specification;

namespace Core.Repository
{
    //where T : BaseEntity constrain to be used only with classes that derive from base entity
    //where T : class constrain to be used only with any classes 
    //EX:so we pass IGenericRepository<product> 
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T> GetEntityWithSpecAsync(ISpecification<T> specification);

        Task<IReadOnlyList<T>> GetListOfEntitiesWithSpectAsync(ISpecification<T> specification);

        Task<int> CountAsync(ISpecification<T> specification);
    }
}