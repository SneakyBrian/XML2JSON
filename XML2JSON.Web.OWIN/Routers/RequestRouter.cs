using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using XML2JSON.Web.OWIN.Handlers;
using XML2JSON.Web.OWIN.Interfaces;

namespace XML2JSON.Web.OWIN.Routers
{
    public class RequestRouter : IRequestHandler, IHasCache
    {
        public async Task HandleRequest(IOwinContext context)
        {
            var remoteRequestHandler = new RemoteRequestHandler { Cache = Cache };
            if (remoteRequestHandler.CanHandleRequest(context))
            {
                await remoteRequestHandler.HandleRequest(context);
                return;
            }

            var submittedDataRequestHandler = new SubmittedDataRequestHandler();
            if (submittedDataRequestHandler.CanHandleRequest(context))
            {
                await submittedDataRequestHandler.HandleRequest(context);
                return;
            }
                        
            var invalidRequestHandler = new InvalidRequestHandler();
            if (invalidRequestHandler.CanHandleRequest(context))
            {
                await invalidRequestHandler.HandleRequest(context);
                return;
            }            
        }

        public ObjectCache Cache { get; set; }

        public bool CanHandleRequest(IOwinContext context)
        {
            return true;
        }
    }
}