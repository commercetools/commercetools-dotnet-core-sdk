using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public abstract class Money : IComparable
    {
        public string Type { get; set; }
        public string CurrencyCode { get; set; }
        public int CentAmount { get; set; }
        public int FractionDigits { get; set; }

        public int CompareTo(object obj)
        {
            Money money = obj as Money;
            if (money == null)
            {
                throw new ArgumentException();
            }
            if (this.CurrencyCode != money.CurrencyCode)
            {
                throw new ArgumentException();
            }
            return this.CentAmount.CompareTo(money.CentAmount);
        }

        // TODO Refactor this, this exists for now only for cart predicates as a facade method
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
            return new CentPrecisionMoney() { CentAmount = (int)amount * 100, CurrencyCode = currencyCode };
        }

        public static bool operator <(Money moneyLeft, Money moneyRight)
        {
            return moneyLeft.CompareTo(moneyRight) < 0;
        }

        public static bool operator >(Money moneyLeft, Money moneyRight)
        {
            return moneyLeft.CompareTo(moneyRight) > 0;
        }

        public static bool operator <=(Money moneyLeft, Money moneyRight)
        {
            return moneyLeft.CompareTo(moneyRight) <= 0;
        }

        public static bool operator >=(Money moneyLeft, Money moneyRight)
        {
            return moneyLeft.CompareTo(moneyRight) >= 0;
        }

        public static bool operator ==(Money moneyLeft, Money moneyRight)
        {
            return moneyLeft.CompareTo(moneyRight) == 0;
        }

        public static bool operator !=(Money moneyLeft, Money moneyRight)
        {
            return moneyLeft.CompareTo(moneyRight) != 0;
        }
    }
}
