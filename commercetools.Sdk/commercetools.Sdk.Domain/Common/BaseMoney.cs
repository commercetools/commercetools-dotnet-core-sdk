using System;

namespace commercetools.Sdk.Domain
{
    public abstract class BaseMoney : IComparable
    {
        public string Type { get; set; }
        public string CurrencyCode { get; set; }
        public int CentAmount { get; set; }
        public int FractionDigits { get; set; }

        public int CompareTo(object obj)
        {
            BaseMoney money = obj as BaseMoney;
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

        public static bool operator <(BaseMoney moneyLeft, BaseMoney moneyRight)
        {
            return moneyLeft.CompareTo(moneyRight) < 0;
        }

        public static bool operator >(BaseMoney moneyLeft, BaseMoney moneyRight)
        {
            return moneyLeft.CompareTo(moneyRight) > 0;
        }

        public static bool operator <=(BaseMoney moneyLeft, BaseMoney moneyRight)
        {
            return moneyLeft.CompareTo(moneyRight) <= 0;
        }

        public static bool operator >=(BaseMoney moneyLeft, BaseMoney moneyRight)
        {
            return moneyLeft.CompareTo(moneyRight) >= 0;
        }

        public static bool operator ==(BaseMoney moneyLeft, BaseMoney moneyRight)
        {
            return moneyLeft.CompareTo(moneyRight) == 0;
        }

        public static bool operator !=(BaseMoney moneyLeft, BaseMoney moneyRight)
        {
            return moneyLeft.CompareTo(moneyRight) != 0;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BaseMoney money))
            {
                throw new ArgumentException();
            }
            if (this.CurrencyCode != money.CurrencyCode)
            {
                throw new ArgumentException();
            }
            return this.CentAmount.Equals(money.CentAmount) && this.CurrencyCode.Equals(money.CurrencyCode);
        }

        public override int GetHashCode()
        {
            return this.CentAmount.GetHashCode() + this.CurrencyCode.GetHashCode();
        }
    }
}
