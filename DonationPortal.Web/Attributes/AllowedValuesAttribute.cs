using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DonationPortal.Web.Attributes
{
	public class AllowedValuesAttribute : ValidationAttribute
	{
		private readonly object[] _allowedValues;
		private readonly Type _valueType;

		public AllowedValuesAttribute(Type valueType, params object[] allowedValues)
		{
			this._valueType = valueType;
			this._allowedValues = allowedValues;
		}

		public override bool IsValid(object value)
		{
			// the required attribute is used for required, not this.
			if (value == null)
			{
				return false;
			}

			if (!(value.GetType() == _valueType))
			{
				return false;
			}

			return _allowedValues.Any(allowedValue => Equals(value, allowedValue));
		}
	}
}