using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using log4net;

namespace DonationPortal.Web.Attributes
{
	public class ExceptionResponseAttribute : ExceptionFilterAttribute
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ExceptionResponseAttribute));

		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			var actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
			var controllerName = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerType.FullName;

			_log.Error(
				string.Format(
					"Unhandled exception processing action {0} for controller {1}.",
					actionName,
					controllerName),
				actionExecutedContext.Exception);

			base.OnException(actionExecutedContext);
		}
	}
}