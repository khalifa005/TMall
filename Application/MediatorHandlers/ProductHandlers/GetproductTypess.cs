using Infrastructure.Data;
using Khalifa.Framework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;

namespace Application.MediatorHandlers.ProductHandlers
{
    public class GetproductTypess
    {
        public class Request : IRequest<Response>
        {
        }

        public class Response : ApiResponse
        {
            public Response()
            {

            }
            public IReadOnlyList<ProductType> Types { get; set; }

            public Response(IReadOnlyList<ProductType> types)
            {
                Types = types;
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

                var proEntities = await _context.ProductTypes
                .ToListAsync(cancellationToken);

                return new Response(proEntities);
            }
        }
    }
}
