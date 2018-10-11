namespace EventApi.Utility
{
    using System;
    using System.Web;
    using System.Web.Http.Filters;

    using Elmah;

    public class ElmahHandleWebApiErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var e = context.Exception;
            RaiseErrorSignal(e);
        }

        private static void RaiseErrorSignal(Exception e)
        {
            var context = HttpContext.Current;
            if (context == null) return;
            var signal = ErrorSignal.FromContext(context);
            signal?.Raise(e, context);
        }
    }
}