using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DonationPortal.Engine.PaymentProcessor
{
	public class CreditCardIssuerDetector
	{
		private static readonly Regex _cardRegex = new Regex("^(?:(?<Visa>4\\d{3})|(?<MasterCard>5[1-5]\\d{2})|(?<Discover>6011)|(?<DinersClub>(?:3[68]\\d{2})|(?:30[0-5]\\d))|(?<Amex>3[47]\\d{2}))([ -]?)(?(DinersClub)(?:\\d{6}\\1\\d{4})|(?(Amex)(?:\\d{6}\\1\\d{5})|(?:\\d{4}\\1\\d{4}\\1\\d{4})))$");

		private static readonly Regex _nonDigitRegex = new Regex(@"[^\d]");

		public CreditCardIssuer? GetIssuer(string creditCardNumber)
		{
			var digitsOnly = _nonDigitRegex.Replace(creditCardNumber, string.Empty);

			var groups = _cardRegex.Match(digitsOnly).Groups;

			return Enum.GetValues(typeof(CreditCardIssuer))
				.Cast<CreditCardIssuer?>()
				.FirstOrDefault(issuer => groups[issuer.ToString()].Success);
		}
	}

	public enum CreditCardIssuer
	{
		MasterCard,
		Visa,
		Discover,
		Amex
	}
}
