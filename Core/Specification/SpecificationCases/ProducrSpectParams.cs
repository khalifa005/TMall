using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification.SpecificationCases
{
    //what we take from the client in controller method
    public class ProductSpectParams
    {
        public const int MaxPageSize = 50;

        private int _pageSize = 6;//default value

        public int PageSize { 
            get =>_pageSize ;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public int PageIndex { get; set; } = 1;

        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string Sort { get; set; }

        public string _search { get; set; }
        public string Search { 
            get => _search;
            set => _search = value.ToLower(); }
    }
}
