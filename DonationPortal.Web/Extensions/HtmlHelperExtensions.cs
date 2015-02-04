using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace DonationPortal.Web.Extensions
{
	public static class HtmlHelperExtensions
	{
		public static bool HasAnyErrors<TModel>(this HtmlHelper<TModel> htmlHelper)
		{
			return !htmlHelper.ViewData.ModelState.IsValid;
		}

		public static bool HasError<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> propertyExpression)
		{
			var metadata = ModelMetadata.FromLambdaExpression(propertyExpression, htmlHelper.ViewData);

			var propertyName = metadata.PropertyName;

			return htmlHelper.ViewData.ModelState[propertyName] != null && htmlHelper.ViewData.ModelState[propertyName].Errors.Any();
		}
	}
}