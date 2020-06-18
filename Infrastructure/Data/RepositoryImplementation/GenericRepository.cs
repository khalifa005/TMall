using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;
using Core.Repository;

namespace Infrastructure.Data.RepositoryImplementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        public System.Threading.Tasks.Task<IReadOnlyList<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
