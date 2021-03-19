using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace GenePlanet.HaveIBeenBreached.RestApiGateway
{
    public static class CompositionRoot
    {
        public static void AddRestApiGateway(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddControllers()!.AddControllersAsServices();
            serviceCollection.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "RestApiGateway", Version = "v1"});
            });
        }

        public static void UseRestApiGateway(this IApplicationBuilder app, WebHostBuilderContext builderContext)
        {
            if (builderContext.HostingEnvironment.IsDevelopment())
            {
                app.UseSwagger(options => options.RouteTemplate = "swagger/{documentName}/swagger.json");
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("swagger/v1/swagger.json", "RestApiGateway v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}