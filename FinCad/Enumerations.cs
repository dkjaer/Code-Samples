namespace FinCad
{
    public class Enumerations
    {
        /// <summary>
        /// 
        /// </summary>
        public enum AccrualMethod
        {
            Actual_365_Fixed = 1,
            Actual_360,
            Actual_365,
            ISDA_30_360,
            Euro_30E_360,
            Euro_30Eplus_360,
            ISMA_Actual_Actual,
            ISDA_Actual_Actual,
            OLD_30_360,
            OLD_30E_360,
            SIA_30_360,
            BMA_30_360,
            German_30_360,
            unknown,
            No_Value
        }

        public enum BusinessDayConvention
        {
            NoAdjustment = 1,
            NextBusinessDay,
            PreviousBusinessDay,
            ModifiedFollowingBusinessDay,
            EOM_NoAdjustment,
            EOM_NextGoodDay,
            EOM_PreviousGoodDay
        }

        public enum ExchangeOfPrincipal
        {
            None = 1,
            FrontAndBack,
            AtMaturity
        }

        public enum InterpolationMethod
        {
            Linear = 1,
            CubicSpline,
            Exponential
        }

        public enum RateCompoundingBasis
        {
            Simple = 1,
            Compounding
        }

        public enum TradePosition
        {
            Long = 1,
            Short
        }

        public enum CompoundingFrequency
        {
            NoValue,
            Annual,
            SemiAnnual,
            Quarterly,
            Monthly,
            Unknown,
            Weekly
        }

        public enum RateQuotationBasis
        {
            Annual = 1,
            SemiAnnual,
            Quarterly,
            Monthly,
            Daily,
            Continuous,
            SimpleInterest,
            DiscountRate
        }

        public enum DateGenerationDirection
        {
            ForwardFromEffectiveDate = 1,
            BackwardFromTerminatingDate
        }

        public enum BootstrappingMethod
        {
            FromDiscountFactorsAndConstantForwardRates = 1,
            FromRatesAndSpliceAssumingConstantForwardRates,
            FromDiscountFactorsAndLinearParSwapRates,
            FromRatesAndSpliceAssumingParSwapRates,
            not_used_a,
            not_used_b,
            ConstantForwardRates_FromDiscountFactorsUsingForwardRatesForGaps
        }

        public enum CurveGenerationMethod
        {
            Cash_Futures_SwapRates = 1,
            Cash_SwapRates,
            Cash_Futures,
            SwapRates
        }

        public enum DateAdjustmentMethod
        {
            MarketDays = 1,
            Days,
            Weeks,
            Months,
            Years,
            Saturday,
            Sunday,
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday
        }
    }
}
