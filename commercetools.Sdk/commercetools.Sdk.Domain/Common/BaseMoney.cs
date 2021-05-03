using System;
using System.Globalization;

namespace commercetools.Sdk.Domain
{
    public abstract class BaseMoney : IComparable
    {
        public virtual MoneyType Type { get; set; }
        public string CurrencyCode { get; set; }
        public long CentAmount { get; set; }
        public int? FractionDigits { get; set; }

        public abstract decimal AmountToDecimal();

        public override string ToString()
        {
            return $"{this.AmountToDecimal()} {CurrencyCode}";
        }

        public int CompareTo(object obj)
        {
            if (!(obj is BaseMoney money))
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
            if (ReferenceEquals(moneyLeft, moneyRight))
            {
                return false;
            }
            if (ReferenceEquals(moneyLeft,null) || ReferenceEquals(moneyRight,null))
            {
                return false;
            }
            return moneyLeft.CompareTo(moneyRight) < 0;
        }

        public static bool operator >(BaseMoney moneyLeft, BaseMoney moneyRight)
        {
            if (ReferenceEquals(moneyLeft, moneyRight))
            {
                return false;
            }
            if (ReferenceEquals(moneyLeft,null) || ReferenceEquals(moneyRight,null))
            {
                return false;
            }
            return moneyLeft.CompareTo(moneyRight) > 0;
        }

        public static bool operator <=(BaseMoney moneyLeft, BaseMoney moneyRight)
        {
            if (ReferenceEquals(moneyLeft, moneyRight))
            {
                return false;
            }
            if (ReferenceEquals(moneyLeft,null) || ReferenceEquals(moneyRight,null))
            {
                return false;
            }
            return moneyLeft.CompareTo(moneyRight) <= 0;
        }

        public static bool operator >=(BaseMoney moneyLeft, BaseMoney moneyRight)
        {
            if (ReferenceEquals(moneyLeft, moneyRight))
            {
                return false;
            }
            if (ReferenceEquals(moneyLeft,null) || ReferenceEquals(moneyRight,null))
            {
                return false;
            }
            return moneyLeft.CompareTo(moneyRight) >= 0;
        }

        public static bool operator ==(BaseMoney moneyLeft, BaseMoney moneyRight)
        {
            if (ReferenceEquals(moneyLeft, moneyRight))
            {
                return true;
            }
            if (ReferenceEquals(moneyLeft,null) || ReferenceEquals(moneyRight,null))
            {
                return false;
            }
            return moneyLeft.CompareTo(moneyRight) == 0;
        }

        public static bool operator !=(BaseMoney moneyLeft, BaseMoney moneyRight)
        {
            if (ReferenceEquals(moneyLeft, moneyRight))
            {
                return false;
            }
            if (ReferenceEquals(moneyLeft,null) || ReferenceEquals(moneyRight,null))
            {
                return true;
            }
            return moneyLeft.CompareTo(moneyRight) != 0;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (ReferenceEquals(obj,null) || ReferenceEquals(this,null))
            {
                return false;
            }
            if (!(obj is BaseMoney money))
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
