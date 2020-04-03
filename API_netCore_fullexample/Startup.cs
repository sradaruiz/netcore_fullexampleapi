using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniriojaREST.Entities;
using UniriojaREST.Services;
using UniriojaREST.Models.Requests;
using UniriojaREST.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Formatters;
using Halcyon.Web.HAL.Json;
using System;
using System.IO;
using Swashbuckle.AspNetCore.Swagger;

namespace UniriojaREST
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IUrlHelper>(x =>
            {
                var actionContext = x.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            // Add framework services.
            services.AddMvc()
            // .AddXmlSerializerFormatters()
            .AddFluentValidation(config =>
                {
                    config.RegisterValidatorsFromAssemblyContaining<Startup>();
                })
            .AddMvcOptions(c => {
                    c.OutputFormatters.RemoveType<JsonOutputFormatter>();
                    c.OutputFormatters.Add(new JsonHalOutputFormatter(
                new string[] { "application/hal+json", "application/vnd.example.hal+json", "application/vnd.example.hal.v1+json" }
                ));
            });
            
            services.AddDbContext<ConcesionariosContext>(o => o.UseInMemoryDatabase("ConcesionariosMemoryDB"));
            services.AddScoped<IClientesRepository, ClientesRepository>();
            
            // Configure Gzip compression
            // services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            // services.AddResponseCompression();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Concesionarios API", Version = "v1" });

                //Set the comments path for the Swagger JSON and UI.
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "ConcesionariosApi.xml"); 
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ConcesionariosContext concesionariosContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Cliente, ClienteResponse>();
                cfg.CreateMap<AddClienteRequest, Entities.Cliente>();
                cfg.CreateMap<UpdateClienteRequest, Entities.Cliente>();
                cfg.CreateMap<Entities.Presupuesto, PresupuestoResponse>();
            });
            
            concesionariosContext.EnsureSeedDataForContext();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
             app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Concesionarios API");
            });

            // Enable Gzip compression
            // app.UseResponseCompression();

            app.UseMvc();
        }
    }
}
