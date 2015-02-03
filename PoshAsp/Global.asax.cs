using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Security.Principal;
using PoshAsp.Models;
using MongoDB.Bson.Serialization;

namespace PoshAsp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            BsonClassMap.RegisterClassMap<Cluster>();
            BsonClassMap.RegisterClassMap<ClusterGroup>();
            BsonClassMap.RegisterClassMap<Computer>();
            BsonClassMap.RegisterClassMap<IpAddress>();
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie TokenCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (TokenCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(TokenCookie.Value);

                try
                {
                    AuthToken Token = new AuthToken(ticket.UserData);
                    GenericPrincipal User = new GenericPrincipal(new GenericIdentity(Token.Username), null);
                    HttpContext.Current.User = User;
                }
                catch
                {
                    GenericPrincipal User = new GenericPrincipal(new GenericIdentity(String.Empty), null);
                    HttpContext.Current.User = User;
                }
            }
        }
    }
}
