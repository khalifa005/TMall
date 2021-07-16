using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Specification
{
    public interface ISpecification<T>
    {
        //filtration and paging will be here 
        Expression<Func<T,object>> OrderBy { get;}
        Expression<Func<T,object>> OrderByDescending { get;}

        //the criteria of the thing we need 
        Expression<Func<T, bool>> Criteria { get; }

        List<Expression<Func<T, object>>> Includes { get; }

        public int Skip { get;}
        public int Take { get;}
        public bool IsPagingEnabled { get;}

    }
}
