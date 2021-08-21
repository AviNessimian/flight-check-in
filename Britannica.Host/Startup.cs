using Autofac;
using Autofac.Extensions.DependencyInjection;
using Britannica.Application;
using Britannica.Host.Filters;
using Britannica.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Encodings.Web;

namespace Britannica.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            var mvcBuilder = services.AddControllers(ControllersSetupAction).AddJsonOptions(ControllersJsonSetupAction);
            services.AddSwaggerGen(SwaggerGenSetupAction);

            IContainer container = BuildContainer(services);
            return new AutofacServiceProvider(container);

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSwagger().UseSwaggerUI(SwaggerUiSetupAction);
            app.UseEndpoints(EndPointSetupAction);

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
            }
        }


        internal static Action<IEndpointRouteBuilder> EndPointSetupAction =>
           endpoints =>
           {
               endpoints.MapControllers();
           };

        private static Action<MvcOptions> ControllersSetupAction =>
            options =>
            {
                options.Filters.Add<ApiExceptionFilterAttribute>();
            };

        private static Action<JsonOptions> ControllersJsonSetupAction =>
            options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Default;
            };

        internal static Action<SwaggerUIOptions> SwaggerUiSetupAction =>
           c =>
           {
               c.RoutePrefix = "";
               c.SwaggerEndpoint("/swagger/v1/swagger.json", "Britannica API V1");
           };

        private static Action<SwaggerGenOptions> SwaggerGenSetupAction =>
            setupAction =>
            {
                setupAction.SwaggerDoc("v1", new OpenApiInfo
                {
                    Contact = new OpenApiContact
                    {
                        Name = "Avi Nessimian",
                        Email = "nessimian.avi@gmail.com",
                        Url = new Uri("https://github.com/AviNessimian"),
                    }
                });
            };

        private static IContainer BuildContainer(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            var assemblyList = new List<Assembly>
            {
                Assembly.GetExecutingAssembly(),
                Application.DI.Assembly
            };

            builder.RegisterApplication(assemblyList);
            builder.RegisterInfrastructure();

            builder.Populate(services);

            var container = builder.Build();
            return container;
        }
    }
}
