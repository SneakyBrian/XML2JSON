using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace XML2JSON.Web.OWIN.Cache
{
    [Serializable]
    public class JsonCacheItem
    {
        public JsonCacheItem(string data)
        {
            Data = data;

            using (var hashAlgo = SHA1Managed.Create())
            {
                Hash = Convert.ToBase64String(hashAlgo.ComputeHash(Encoding.UTF8.GetBytes(data)));
            }
        }

        public string Data
        {
            get;
            private set;
        }

        public string Hash
        {
            get;
            private set;
        }
    }
}