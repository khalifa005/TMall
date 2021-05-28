using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using API.Helper;
using API.Middleware;
using Application.MediatorHandlers.ProductHandlers;
using Core.Repository;
using Infrastructure.Data;
using Infrastructure.Data.RepositoryImplementation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(string.Format(@"{0}\TMallDocumentationApi.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TMallDocumentationApi",
                });

            });
            #endregion

            services.AddMediatR(typeof(GetProduct).Assembly);

            services.AddDbContext<StoreContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("SqlServerConnection")));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddAutoMapper(typeof(MappingProfiles));//define where the assembles

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddControllers().AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            //here we will override the [APIController] behavior and it should come after AddControllers
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                //so if the mode stat is not valid 
                opt.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                    .Where(x=> x.Value.Errors.Count > 0)
                    .SelectMany(x=> x.Value.Errors)
                    .Select(x=> x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //using our custom middleware
            app.UseMiddleware<ExceptionMiddleware>();

            if (env.IsDevelopment())
            {

                #region Swagger
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TMall");
                });
                #endregion
            }

            //if no endpoint matches then redirect to
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();



            app.UseRouting();

            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Products", action = "GetProducts" });

                endpoints.MapControllers();
            });
        }
    }
}
