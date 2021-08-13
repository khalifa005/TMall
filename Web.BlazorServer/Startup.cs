using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Application.AppMappingProfiles;
using Application.MediatorHandlers.ProductHandlers;

namespace Web.BlazorServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(GetProduct).Assembly);

            services.AddDbContext<StoreContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("SqlServerConnection")), ServiceLifetime.Transient);


            //services.AddAutoMapper(typeof(MappingProfiles));//define where the assembles
            services.AddAutoMapper(typeof(AppProfile));//define where the assembles

            services.AddControllers();//bring some Parts from .Net mvc like Identity .. login

            services.AddLocalization(options => options.ResourcesPath = "Resources");//put all translation inside this folder
            services.AddPortableObjectLocalization(options => options.ResourcesPath = "Localization");
            
            services.AddRazorPages();
            services.AddServerSideBlazor();
        }

        public RequestLocalizationOptions GetLocalizationOptions()
        {
            var cultures = _configuration.GetSection("Cultures")
                .GetChildren().ToDictionary(x=> x.Key, x=> x.Value);

            var supportedCultures = cultures.Keys.ToArray();

            var localizationOptions = new RequestLocalizationOptions()
                .AddSupportedCultures(supportedCultures)//data format date , numbers
                .AddSupportedUICultures(supportedCultures);
            return localizationOptions;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRequestLocalization(GetLocalizationOptions());
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();//to know how to go to certain path//if we want to have our controllelrs here
                endpoints.MapBlazorHub(); 
                //MapBlazorHub sets up the endpoint for the SignalR connection with the client browser.
                 endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
