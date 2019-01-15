namespace commercetools.Sdk.Domain
{
    using System;

    public class Money : BaseMoney
    {
        // TODO Check if this is good, this exists for now only for cart predicates
        public static Money Parse(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException();
            }
            string[] splitInput = input.Split(' ');
            if (splitInput.Length != 2)
            {
                throw new ArgumentException();
            }
            double amount = double.Parse(splitInput[0]);
            string currencyCode = splitInput[1];
            return new Money() { CentAmount = (int)amount * 100, CurrencyCode = currencyCode };
        }
    }
}