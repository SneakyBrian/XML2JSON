using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using XML2JSON.Core;
using XML2JSON.Web.OWIN.Interfaces;

namespace XML2JSON.Web.OWIN.Handlers
{
    public class SubmittedDataRequestHandler : IRequestHandler
    {
        public async Task HandleRequest(IOwinContext context)
        {
            var callback = context.Request.Query["callback"];
            var xml = string.Empty;

            using (var streamReader = new StreamReader(context.Request.Body))
            {
                xml = await streamReader.ReadToEndAsync();
            }

            var json = await Converter.ConvertToJsonAsync(xml);

            var result = string.Empty;

            if (string.IsNullOrWhiteSpace(callback))
            {
                context.Response.ContentType = "application/json";
                result = json;
            }
            else
            {
                context.Response.ContentType = "application/javascript";
                result = callback + "(" + json + ");";
            }

            await context.Response.WriteAsync(result);
        }


        public bool CanHandleRequest(IOwinContext context)
        {
            return context.Request.Method.ToUpperInvariant() == "POST";
        }
    }
}