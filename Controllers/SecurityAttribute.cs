using MVCtutorial.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Web.Security;

public class SecurityAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorized = base.AuthorizeCore(httpContext);
            if (!authorized) {
                return false;
            }
        
        return true;
        }
    }
