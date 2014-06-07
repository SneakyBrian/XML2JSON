using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using XML2JSON.Core;
using XML2JSON.Web.OWIN.Cache;
using XML2JSON.Web.OWIN.Interfaces;

namespace XML2JSON.Web.OWIN.Handlers
{
    public class RemoteRequestHandler : IRequestHandler, IHasCache
    {
        private const int CACHE_DURATION_MINS = 15;

        public async Task HandleRequest(IOwinContext context)
        {
            var uri = context.Request.Query["uri"];
            var callback = context.Request.Query["callback"];
            var xml = string.Empty;

            //see if we have the result cached
            var jsonCache = Cache.Get(uri) as JsonCacheItem;

            if (jsonCache == null)
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    xml = await response.Content.ReadAsStringAsync();
                }

                var json = await Converter.ConvertToJsonAsync(xml);

                jsonCache = new JsonCacheItem(json);

                //cache it...
                Cache.Add(uri, jsonCache, DateTimeOffset.Now.AddMinutes(CACHE_DURATION_MINS));
            }
            //check If-None-Match header etag to see if it matches our data hash
            else if (context.Request.Headers["If-None-Match"] == String.Concat("\"", jsonCache.Hash, "\""))
            {
                //if it does return 304 Not Modified
                context.Response.StatusCode = 304;
                return;
            }

            var result = string.Empty;

            if (string.IsNullOrWhiteSpace(callback))
            {
                context.Response.ContentType = "application/json";
                result = jsonCache.Data;
            }
            else
            {
                context.Response.ContentType = "application/javascript";
                result = callback + "(" + jsonCache.Data + ");";
            }

            

            //set the response as cached for cache duration
            context.Response.Headers["Cache-Control"] = string.Format("max-age={0}", CACHE_DURATION_MINS * 60);

            //set etag
            context.Response.ETag = String.Concat("\"", jsonCache.Hash, "\"");

            await context.Response.WriteAsync(result);
        }

        public ObjectCache Cache { get; set; }

        public bool CanHandleRequest(IOwinContext context)
        {
            //get request, and we have uri parameter
            return context.Request.Method.ToUpperInvariant() == "GET" && !string.IsNullOrWhiteSpace(context.Request.Query["uri"]);
        }
    }
}