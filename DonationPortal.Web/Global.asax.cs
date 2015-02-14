using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using log4net;

namespace DonationPortal.Web
{
    public class Global : HttpApplication
    {
	    private static readonly ILog _log = LogManager.GetLogger(typeof (Global));

	    public override void Init()
		{
			Error += Global_Error;
		    base.Init();
	    }

	    void Application_Start(object sender, EventArgs e)
        {
	        log4net.Config.XmlConfigurator.Configure();

            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

        }

		void Global_Error(object sender, EventArgs e)
		{
			var application =  sender as HttpApplication;
			var context = application.Context;
			var exception = context.Server.GetLastError();

			_log.Error("Unhandled application error.", exception);
		}
    }
}