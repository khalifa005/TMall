using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Repository;
using Core.Specification;
using Infrastructure.Data.SpecificationEvaluator;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.RepositoryImplementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;

        public GenericRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }


        //above is before using specification pattern we can't do any filtering or sorting or get related data

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            //5 spec

            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
        }

        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> specification)
        {
            //4 spec

            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> GetListOfEntitiesWithSpectAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).CountAsync();
        }
    }
}
