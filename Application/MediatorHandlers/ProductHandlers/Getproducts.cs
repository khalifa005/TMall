using FluentValidation;
using Infrastructure.Data;
using Khalifa.Framework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;

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
}
