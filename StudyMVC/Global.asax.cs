using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StudyMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {   //Devide larger applications into multiple area
            //AreaRegistration.RegisterAllAreas();

            //Prefer routing based on local attributes
            RouteTable.Routes.MapMvcAttributeRoutes();
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
