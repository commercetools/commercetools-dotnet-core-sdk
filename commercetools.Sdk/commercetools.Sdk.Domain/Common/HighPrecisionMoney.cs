﻿using System;

namespace commercetools.Sdk.Domain
{
    [MoneyType(Type = MoneyType.HighPrecision)]
    public class HighPrecisionMoney : BaseMoney
    {
        public override MoneyType Type => MoneyType.HighPrecision;
        public long PreciseAmount { get; set; }

        public override decimal AmountToDecimal()
        {
            return (decimal)PreciseAmount / (decimal)Math.Pow(10, FractionDigits.GetValueOrDefault() );
        }

        public static HighPrecisionMoney FromDecimal(string currencyCode, decimal value, int fractionDigits, MidpointRounding midpointRounding = MidpointRounding.AwayFromZero)
        {
            var amount = Math.Round(value * (decimal) Math.Pow(10, fractionDigits), 0, midpointRounding);
            var centAmount = Math.Round(value * 100, 0, midpointRounding);
            return new HighPrecisionMoney
            {
                CurrencyCode = currencyCode,
                PreciseAmount = (long)amount,
                CentAmount = (long)centAmount,
                FractionDigits = fractionDigits
            };
        }
    }
}
