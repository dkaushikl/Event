using EventApi;

using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace EventApi
{
    using System.Web.Http;

    using Swashbuckle.Application;

    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration.EnableSwagger(c => { c.SingleApiVersion("v1", "EventApi"); })
                .EnableSwaggerUi(c => { });
        }
    }
}