using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Khalifa.Framework
{
    public static class FilterUI
    {
        public static IEnumerable<int> GetRange(int from, int to)
        {
            //You add +1 because you want to include the last year, or in case fromYear == toYear
            return Enumerable.Range(new int[] { @from, to }.Min(), Math.Abs(@from - to) + 1);
        }

        public static int? NullifyIfMatchAllOption(int? value)
        {
            if (!value.HasValue) return value;

            if (value.Value == Config.AllOptionValue) return null;

            return value;
        }

        public static long? NullifyIfMatchAllOption(long? value)
        {
            if (!value.HasValue) return value;

            if (value.Value == Config.AllOptionValue) return null;

            return value;
        }

        public static short? NullifyIfMatchAllOption(short? value)
        {
            if (!value.HasValue) return value;

            if (value.Value == Config.AllOptionValue) return null;

            return value;
        }

        public static DateTime? NullifyIfMatchAllOption(DateTime? value)
        {
            if (!value.HasValue) return value;

            return value;
        }

        public static string? NullifyIfMatchAllOption(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            if (value == Config.AllOptionValueString)
                return null;

            return value;
        }

        public static bool? NullifyIfMatchAllOptionBoolean(int? value)
        {
            if (!value.HasValue || value.Value == Config.AllOptionValue) return null;

            return Convert.ToBoolean(value.Value);
        }
    }
    public interface IQuerySetMany<T>
    {
        int Count { get; }

        bool IsFound { get; }

        bool IsNotFound { get; }

        List<T> Items { get; }

        Exception Exception { get; }
    }
    public interface IQuerySetPaging<T> : IQuerySetMany<T>
    {
        PagingInfo PagingInfo { get; }
    }
    public interface IQuerySetOne<out T>
    {
        bool IsFound { get; }

        bool IsNotFound { get; }

        T Item { get; }

        Exception Exception { get; }
    }
    public class QuerySetOne<T> : QuerySet<T>, IQuerySetOne<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:QuerySingle"/> class.
        /// </summary>
        public QuerySetOne()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:QuerySingle"/> class.
        /// </summary>
        public QuerySetOne(T item)
            : base(single: item)
        {
        }

        T IQuerySetOne<T>.Item => this.Single;
    }
    public static class QuerySet
    {
        public static IQuerySetOne<T> One<T>(T item) where T : class
            => new QuerySetOne<T>(item);

        public static IQuerySetMany<T> Many<T>(List<T> items) where T : class
            => new QuerySetMany<T>(items);

        public static QueryResults<T> Paging<T>(List<T> results, int total) where T : class
            => new QueryResults<T>(results, total);
    }

    public class QueryResults<T> where T : class
    {
        public List<T> Items { get; private set; }

        public int TotalResults { get; private set; }

        public QueryResults(List<T> res, int totalResults)
        {
            Items = res;
            TotalResults = totalResults;
        }

        public IQuerySetPaging<T> GetPagingInfo(int page, int pageSize)
            => new QuerySetPaging<T>(Items, page, pageSize, TotalResults);
    }
    public enum QuerySetType
    {
        None,
        Single,
        Multiple,
        QueryError
    }
    public class QuerySet<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:QuerySet"/> class.
        /// By ignoring all the optional parameters, the query set is in status of NotFound
        /// </summary>
        /// <param name="single">Set a value here if a single value is returned</param>
        /// <param name="multiple">Set a value here if a list of value is returned</param>
        public QuerySet(T single = null, List<T> multiple = null)
        {
            if (single == null && multiple == null)
                NotFound();
            else if (single != null)
                Found(single);
            else
                Found(multiple);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:QuerySet"/> class.
        /// </summary>
        public QuerySet() => Type = QuerySetType.None;

        /// <summary>
        /// Gets or sets the type of the query set.
        /// </summary>
        /// <value>The type of the query set.</value>
        public QuerySetType Type { get; private set; }

        public void NotFound()
        {
            Type = QuerySetType.None;
        }

        private T _single;

        public T Single => _single;

        protected List<T> _multiple;

        public List<T> Multiple => _multiple;

        /// <summary>
        /// Query returns one result
        /// </summary>
        /// <param name="single"></param>
        public void Found(T single)
        {
            if (single == null)
            {
                NotFound();
            }
            else
            {
                Type = QuerySetType.Single;
                _single = single;
            }
        }

        /// <summary>
        /// Query returns one or more result
        /// </summary>
        /// <param name="multiple"></param>
        public void Found(List<T> multiple)
        {
            if (multiple == null)
            {
                NotFound();
            }
            else if (multiple.Count == 0)
            {
                _multiple = multiple;
                NotFound();
            }
            else
            {
                Type = QuerySetType.Multiple;
                _multiple = multiple;
            }
        }

        private Exception _ex;

        public Exception Exception => _ex;

        public void Error(Exception ex)
        {
            _ex = ex;
            Type = QuerySetType.QueryError;
        }

        public int Count
        {
            get
            {
                switch (Type)
                {
                    case QuerySetType.None: return 0;
                    case QuerySetType.Single: return 1;
                    case QuerySetType.Multiple: return _multiple.Count;
                    default: return 0;
                }
            }
        }

        public bool IsFound => Type != QuerySetType.None && Type != QuerySetType.QueryError;

        public bool IsNotFound => !IsFound;
    }
    public class QuerySetMany<T> : QuerySet<T>, IQuerySetMany<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:QueryMulti"/> class.
        /// </summary>
        public QuerySetMany()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:QueryMulti"/> class.
        /// </summary>
        public QuerySetMany(List<T> items)
            : base(multiple: items)
        {
        }

        List<T> IQuerySetMany<T>.Items => this.Multiple;
    }
    public class QuerySetPaging<T> : QuerySetMany<T>, IQuerySetPaging<T> where T : class
    {
        /// <summary>
        /// Gets or sets the paging informatino.
        /// </summary>
        /// <value>The paging informatino.</value>
        public PagingInfo PagingInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:QuerySetPaging"/> class.
        /// </summary>
        public QuerySetPaging()
            : base()
        {
        }

        public QuerySetPaging(List<T> items, int currentPage, int pageSize, int totalResults)
            : base(items)
        {
            PagingInfo = new PagingInfo
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalResults = totalResults
            };
        }
    }
    
    public enum OrderOperator
    {
        None = 0,
        Ascending,
        Descending
    }

    public abstract class FilterUI<T> where T : class
    {
        public M DeserializeJson<M>(string json)
        {
            var unescaped = Compression.Decompress(Uri.UnescapeDataString(json));

            using (var input = new StringReader(unescaped))
            {
                return Json.Deserialize<M>(input.ReadToEnd());
            }
        }

        public abstract T GetFilter();

        public string SerializeJson(object obj)
        {
            var queryString = Json.Serialize(obj);

            return Uri.EscapeDataString(Compression.Compress(queryString));
        }

        public bool IsEmpty()
        {
            foreach (var pi in GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    var value = (string?)pi.GetValue(this);
                    if (!string.IsNullOrEmpty(value)) return false;
                }
                else if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    var val = pi.GetValue(this);
                    if (val != null) return false;
                }
            }

            return true;
        }

        public abstract string SerializeJson();
    }

    public interface IFilter<T>
    {
        IQueryable<T> Filter(IQueryable<T> query);
    }


    public interface ISort<T>
    {
        IOrderedQueryable<T> Sort(IQueryable<T> query);
    }


    public class QueryCondition<T>
    {
        public IFilter<T> Filter { get; set; }

        public QueryCondition()
        {
        }

        public QueryCondition(IFilter<T> f)
        {
            Filter = f;
        }
    }


    public class PagingQueryCondition<T> : QueryCondition<T>
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public PagingQueryCondition()
        {
        }

        public PagingQueryCondition(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        public PagingQueryCondition(IFilter<T> f, int page, int pageSize) : base(f)
        {
            Page = page;
            PageSize = pageSize;
        }
    }

    public class SortedPagingQueryCondition<T> : PagingQueryCondition<T>
    {
        public ISort<T> Sorter { get; set; }

        public SortedPagingQueryCondition()
        {
        }

        public SortedPagingQueryCondition(IFilter<T> f, ISort<T> s, int page, int pageSize) : base(f, page, pageSize)
        {
            Sorter = s;
        }
    }

    public static class QueryBuilder
    {
        public static PagingSpec<T> Paging<T>(IQueryable<T> query, PagingQueryCondition<T> qi) where T : class
        {
            //pass query from _context.activity.include ...AsiQuerable for filtering with related data 
            if (qi.Filter != null)
                query = qi.Filter.Filter(query);

            var total = query;
            var result = query.Take(qi.PageSize).Skip((qi.Page - 1) * qi.PageSize);

            return new PagingSpec<T>
            {
                Count = total,
                Listing = result
            };
        }

        public static PagingSpec<T> Paging<T>(IQueryable<T> query, SortedPagingQueryCondition<T> qi) where T : class
        {
            if (qi.Sorter == null)
                throw new ArgumentNullException($"{nameof(qi.Sorter)} cannot be null");

            if (qi.Filter != null)
                query = qi.Filter.Filter(query);

            var sorted = qi.Sorter.Sort(query);

            var total = sorted;
            var result = sorted.Take(qi.PageSize).Skip((qi.Page - 1) * qi.PageSize);

            return new PagingSpec<T>
            {
                Count = total,
                Listing = result
            };
        }
    }

    public class ManySpec<T>
    {
        public IQueryable<T> Listing { get; set; }
    }

    public class PagingSpec<T> : ManySpec<T>
    {
        public IQueryable<T> Count { get; set; }
    }

    
}
