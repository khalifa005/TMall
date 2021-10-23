using Infrastructure.Data;
using Khalifa.Framework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;
using System.Linq;
using Application.AppDtos;
using AutoMapper;

namespace Application.MediatorHandlers.ProductHandlers
{

    public class Getproducts
    {
        public class Request : IRequest<Response>
        {
        }

        public class Response : ApiResponse
        {
            public Response()
            {

            }
            public IReadOnlyList<Product> Products { get; set; }

            public Response(IReadOnlyList<Product> products)
            {
                Products = products;
                StatusCode = (int)HttpStatusCode.OK;
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly StoreContext _context;

            public Handler(StoreContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {

                var proEntities = await _context.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .ToListAsync(cancellationToken);

                return new Response(proEntities);
            }
        }
    }

    //for syncfusion component to call it automatically
    public class GetproductsByAdabtor
    {
        public record Request(int Skip, int Take) : IRequest<Response>
        {

        }

        public class Response : ApiResponse
        {
            public Response()
            {

            }
            public IReadOnlyList<ProductAppDto> Products { get; set; }
            public int TotalItem { get; set; }

            public Response(IReadOnlyList<ProductAppDto> products, int totalItem)
            {
                Products = products;
                TotalItem = totalItem;
                StatusCode = (int)HttpStatusCode.OK;
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly StoreContext _context;
            private readonly IMapper _mapper;
            public Handler(StoreContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {

                var proEntities = await _context.Products.Skip(request.Skip).Take(request.Take)
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .ToListAsync(cancellationToken);

                var totalItems = await _context.Products.CountAsync(cancellationToken);

                var mappedProducts = _mapper.Map<List<Product>, List<ProductAppDto>>(proEntities);
                return new Response(mappedProducts, totalItems);
            }
        }
    }
    public class ProductFilterUI : FilterUI<ProductFilter>
    {
        public int BrandId { get; set; }
        public int TypeId { get; set; }
        public string ProductName { get; set; }

        public override ProductFilter GetFilter()
        {
            return new ProductFilter
            {
                BrandId = FilterUI.NullifyIfMatchAllOption(BrandId), //will return null if input with default
                TypeId = FilterUI.NullifyIfMatchAllOption(TypeId),
            };
        }

        public override string SerializeJson() => Json.Serialize(this);

        public static ProductFilterUI Default()
        {
            return new ProductFilterUI
            {
                ProductName = string.Empty,
                BrandId = Config.AllOptionValueShort,
                TypeId = Config.AllOptionValueShort,
            };
        }
    }

    public class ProductFilter : IFilter<Product>
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }

        public string ProductName { get; set; }

        public IQueryable<Product> Filter(IQueryable<Product> query)
        {
            //build query expression tree
            if (BrandId.HasValue)
            {
                query = query.Where(x => x.ProductBrandId == BrandId);
            }

            if (TypeId.HasValue)
            {
                query = query.Where(x => x.ProductTypeId == TypeId);
            }

            if (ProductName is { Length: > 2 })
            {
                ProductName = ProductName.ToLower();

                query = query.Where(x => x.Name.ToLower().Contains(ProductName));
            }

            return query;
        }

    }

    public class ProductSorter : ISort<Product>
    {
        public OrderOperator? ByCreateDate { get; set; }
        public OrderOperator? ByPrice { get; set; }

        public IOrderedQueryable<Product> Sort(IQueryable<Product> query)
        {
            if (ByCreateDate.HasValue)
            {
                // We are sorting by Id because well Id indexes very fast in sorting than date and we know bigger id means created later
                return ByCreateDate switch
                {
                    OrderOperator.Descending => query.OrderByDescending(x => x.Id),
                    _ => query.OrderBy(x => x.Id)
                };
            }

            if (ByPrice.HasValue)
            {
                return ByPrice switch
                {
                    OrderOperator.Descending => query.OrderByDescending(x => x.Price),
                    _ => query.OrderBy(x => x.Price)
                };
            }

            return query.OrderByDescending(x => x.Id);
        }

        public static ProductSorter ByCreateDateAsc() => new() { ByCreateDate = OrderOperator.Ascending };

        public static ProductSorter ByCreateDateDesc() => new() { ByCreateDate = OrderOperator.Descending };

        public static ProductSorter ByPricrDesc() => new() { ByPrice = OrderOperator.Descending };
        public static ProductSorter ByPricrAsc() => new() { ByPrice = OrderOperator.Ascending };
    }

    public class GetProductsWithPagining
    {
        public record Request(ProductFilter Filter, ISort<Product> Sorter, int Page, int PageSize) : IRequest<Response>;

        public class Response : ApiResponse
        {
            //we can introduce paging info and send it from the service rather IQuerySetPaging
            public IQuerySetPaging<ProductAppDto> Data { get; set; }
            public List<ProductAppDto> TestData { get; set; } = new();
            public PagingInfo Info { get; set; }

            public Response()
            {
            }

            public Response(IQuerySetPaging<ProductAppDto> data, List<ProductAppDto> testData, PagingInfo info)
            {
                Data = data;
                Info = info;
                TestData = testData;
                StatusCode = (int)HttpStatusCode.OK;
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IMapper _mapper;
            private readonly StoreContext _context;

            public Handler(StoreContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
              

                try
                {
                    var pagingQuery = new SortedPagingQueryCondition<Product>(request.Filter, request.Sorter, request.Page, request.PageSize);

                    var queryRelatedData = _context.
                        Products
                        .Include(x=> x.ProductType)
                        .Include(x=> x.ProductBrand)
                        .AsQueryable();
                    
                    var query = QueryBuilder.Paging(queryRelatedData, pagingQuery);

                    var count = await query.Count.CountAsync(cancellationToken);

                    //include query.Listing

                    var result = await query.Listing.ToListAsync(cancellationToken);
                    //map

                    var mappedProducts = _mapper.Map<List<Product>, List<ProductAppDto>>(result);

                    var resultResponse = QuerySet.Paging(mappedProducts, count).GetPagingInfo(request.Page, request.PageSize);
                    var testResponse = resultResponse.Items;
                    var info = resultResponse.PagingInfo;
                    return new Response(resultResponse, testResponse, info);
                }
                catch (Exception ex)
                {
                    return ApiResponse.Error<Response>("get-products-error", ex);
                }
            }
        }
    }
}
