using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace MvcHomework01
{
    public class RunningTimeCounter : ActionFilterAttribute
    {
        Stopwatch watch = new Stopwatch();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            watch.Start();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            watch.Stop();
            Debug.WriteLine($"Time cost: { watch.Elapsed.TotalMilliseconds.ToString() }");
        }
    }
}