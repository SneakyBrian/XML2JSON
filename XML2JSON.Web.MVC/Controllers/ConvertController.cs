using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using XML2JSON.Core;
using XML2JSON.Web.MVC.Models.Caching;

namespace XML2JSON.Web.MVC.Controllers
{
    public class ConvertController : ApiController
    {
        private const int CACHE_DURATION_MINS = 15;

        private static ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }


        /// <summary>
        /// Gets the xml data at the specified uri and converts it to json before returning it
        /// </summary>
        /// <param name="uri">uri of xml data</param>
        /// <param name="callback">javascript callback for jsonp usage. optional</param>
        /// <returns>json encoded data</returns>
        /// <remarks>test client url: http://jsfiddle.net/s7UGv/ </remarks>
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<HttpResponseMessage> Get(string uri, string callback = null)
        {
            //see if we have the result cached
            var jsonCache = Cache.Get(uri) as JsonCacheItem;

            //if we don't..
            if (jsonCache == null)
            {
                //download it
                using (HttpClient httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    var xml = await response.Content.ReadAsStringAsync();

                    //convert it...
                    var json = await Converter.ConvertToJsonAsync(xml);

                    jsonCache = new JsonCacheItem(json);

                    //cache it...
                    Cache.Add(uri, jsonCache, DateTimeOffset.Now.AddMinutes(CACHE_DURATION_MINS));
                }
            }

            string result;

            if (string.IsNullOrWhiteSpace(callback))
            {
                result = jsonCache.Data;
            }
            else
            {
                result = callback + "(" + jsonCache.Data + ");";
            }

            var responseMessage = new HttpResponseMessage
            {
                Content = new StringContent(result)               
            };

            //tell the client to cache for CACHE_DURATION_MINS
            responseMessage.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromMinutes(CACHE_DURATION_MINS),
                NoCache = false,
                Private = false                
            };

            responseMessage.Headers.ETag = new EntityTagHeaderValue(String.Concat("\"", jsonCache.Hash, "\""));

            return responseMessage;        
        }

        /// <summary>
        /// encodes the posted xml data in json format and returns it
        /// </summary>
        /// <param name="xml">xml data</param>
        /// <param name="callback">javascript callback for jsonp usage. optional</param>
        /// <returns>json encoded data</returns>
        public async Task<HttpResponseMessage> Post([FromBody]string xml, string callback = null)
        {
            var json = await Converter.ConvertToJsonAsync(xml);

            string result;

            if (string.IsNullOrWhiteSpace(callback))
            {
                result = json;
            }
            else
            {
                result = callback + "(" + json + ");";
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(result)
            };
        }
    }
}