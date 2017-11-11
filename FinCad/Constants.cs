using System;

namespace FinCad
{
    class Constants
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DateTime NullDate = DateTime.MinValue;

        //// ******************** Cusip-Related

        public const string TRADE_SUFFIX_GOVERNMENT = "GOVT";
        public const string TRADE_SUFFIX_MUNICIPAL = "MUNI";
        public const string TRADE_SUFFIX_CORPORATE = "CORP";
        public const string TRADE_SUFFIX_EQUITY = "EQTY";
        public const string TRADE_SUFFIX_MORTGAGE = "MTGE";
        public const string TRADE_SUFFIX_INDEX = "Index";
        public const string TRADE_SUFFIX_COMDTY = "COMDTY";

        public const int FREQUENCY_ANNUAL = 1;
        public const int FREQUENCY_SEMI_ANNUAL = 2;
        public const int FREQUENCY_QUARTERLY = 4;
        public const int FREQUENCY_MONTHLY = 12;
        public const int FREQUENCY_WEEKLY = 52;

        public const int BASIS_30_360 = 0;
        public const int BASIS_ACTUAL_ACTUAL_365 = 1;
        public const int BASIS_ACTUAL_360 = 2;
        public const int BASIS_ACTUAL_ACTUAL_ISMA = 3;
    }
}
