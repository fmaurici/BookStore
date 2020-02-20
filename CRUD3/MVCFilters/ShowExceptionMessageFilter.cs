using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CRUD3.MVCFilters
{
    public class ShowExceptionMessageFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            filterContext.Result = new JsonResult(
            new
            {
                error = filterContext.Exception.Message.ToString()
            });

            //if (filterContext.Exception != null)
            //{
            //    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //    filterContext.Result = new JsonResult(
            //    new {
            //        Data = new
            //        {
            //            filterContext.Exception.Message,
            //            filterContext.Exception.StackTrace
            //        }
            //    });

            //    filterContext.ExceptionHandled = true;
            //}
            //else
            //{
            //    base.OnException(filterContext);
            //}
        }
    }
}
