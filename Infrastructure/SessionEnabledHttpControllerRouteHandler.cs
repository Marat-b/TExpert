﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.WebHost;
using System.Web.Routing;


namespace TExp.Infrastructure
{
    
        public class SessionEnabledHttpControllerRouteHandler : HttpControllerRouteHandler
        {
            protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
            {
                return new SessionEnabledControllerHandler(requestContext.RouteData);
            }
        }

    
}