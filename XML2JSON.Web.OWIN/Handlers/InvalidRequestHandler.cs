using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using XML2JSON.Web.OWIN.Interfaces;

namespace XML2JSON.Web.OWIN.Handlers
{
    public class InvalidRequestHandler : IRequestHandler
    {
        public async Task HandleRequest(IOwinContext context)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Bad Request");
        }


        public bool CanHandleRequest(IOwinContext context)
        {
            return true;
        }
    }
}