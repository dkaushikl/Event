namespace EventApi
{
    using System;
    using System.Web;
    using System.Web.Http;

    using Newtonsoft.Json;

    public class WebApiApplication : HttpApplication
    {
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.HttpMethod != "OPTIONS")
            {
                return;
            }

            HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
            HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
            HttpContext.Current.Response.End();
        }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling =
            //    ReferenceLoopHandling.Ignore;
            //GlobalConfiguration.Configuration.Formatters.Remove(
            //    GlobalConfiguration.Configuration.Formatters.XmlFormatter);

            UnityContainerRegistration.InitialiseContainer();
        }
    }
}