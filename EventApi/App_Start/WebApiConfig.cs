﻿namespace EventApi
{
    using System.Web.Http;

    using EventApi.Utility;
    using EventApi.Utility.JwtToken;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();
            config.MapHttpAttributeRoutes();

            config.Filters.Add(new JwtAuthenticationAttribute());
            config.Filters.Add(new ElmahHandleWebApiErrorAttribute());

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
        }
    }
}