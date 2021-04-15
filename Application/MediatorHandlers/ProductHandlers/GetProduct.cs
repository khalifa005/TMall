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
   
    public class GetProduct
    {
        public class Request : IRequest<Response>
        {
            public int Id { get; set; }
        }

        public class Response : ApiResponse
        {
            public Response()
            {

            }
            public Product Product { get; set; }

            public Response(Product product)
            {
                Product = product;
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

                var proEntity = await _context.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(p => p.Id == request.Id);
                
                return new Response(proEntity);
            }
        }
    }
}
