using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace XML2JSON.Web.OWIN.Interfaces
{
    interface IRequestHandler
    {
        Task HandleRequest(IOwinContext context);

        bool CanHandleRequest(IOwinContext context);
    }
}
