using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace XML2JSON.Web.OWIN.Interfaces
{
    interface IHasCache
    {
        ObjectCache Cache { get; set; }
    }
}
