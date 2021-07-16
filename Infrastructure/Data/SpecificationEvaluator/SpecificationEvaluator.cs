using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Core.Entities;
using Core.Specification;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.SpecificationEvaluator
{
    public class SpecificationEvaluator<TEntity> where TEntity:BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            //6 spec

            var query = inputQuery;

            if (spec.Criteria != null)
                query = query.Where(spec.Criteria); //  spec.Criteria is p=> p.ProductId == id
            
            
            if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy); 
            
            
            if (spec.OrderByDescending != null)
                query = query.OrderByDescending(spec.OrderByDescending); 
            
            if (spec.IsPagingEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take); 


            //take more multiple include expression then aggregate them and pass them into our query
            query = spec.Includes.Aggregate(query, (entities, expression) => entities.Include(expression));


            return query;

        }
    }
}
