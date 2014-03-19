using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using XML2JSON.Core;

namespace XML2JSON.Web.MVC.Controllers
{
    public class ConvertController : ApiController
    {
        /// <summary>
        /// Gets the xml data at the specified uri and converts it to json before returning it
        /// </summary>
        /// <param name="uri">uri of xml data</param>
        /// <param name="callback">javascript callback for jsonp usage. optional</param>
        /// <returns>json encoded data</returns>
        public async Task<HttpResponseMessage> Get(string uri, string callback = null)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(uri);
                var xml = await response.Content.ReadAsStringAsync();

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

        /// <summary>
        /// encodes the posted xml data in json format and returns it
        /// </summary>
        /// <param name="xml">xml data</param>
        /// <param name="callback">javascript callback for jsonp usage. optiona</param>
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