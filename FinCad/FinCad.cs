using System;
using System.IO;
using System.Runtime.InteropServices;

namespace FinCad
{
    /// <summary>
    /// 
    /// </summary>
    public class Fincad_table_301
    {
        public double EffectiveDate;
        public double TerminatingDate;
        public double Rate;
        public double RateQuotationBasis;
        public double AccrualMethodForOutputRates;
        public double UseThisPoint;
    }

    /// <summary>
    /// 
    /// </summary>
    public class Fincad_table_302
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime EffectiveDate
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime TerminatingDate
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public double Rate
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Enumerations.RateQuotationBasis RateQuotationBasis
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Enumerations.AccrualMethod AccrualMethodForOutputRates
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Enumerations.BusinessDayConvention BusinessDayConvention
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public double UseThisPoint
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Fincad_ratedf_obj_305
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime Date
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public double DiscountFactor
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public double Rate
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FinCad
    {
        bool errorOccurred = false;
        string errorMessage = string.Empty;
        bool isFound = false;
        double[] holidayList = { 1 };
        Enumerations.BusinessDayConvention businessDayConvention =
            Enumerations.BusinessDayConvention.ModifiedFollowingBusinessDay;

        /// <summary>
        /// 
        /// </summary>
        public bool ErrorOccurred
        {
            get
            {
                return errorOccurred;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Isfound
        {
            get
            {
                return isFound;
            }

            set
            {
                isFound = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double[] HolidayList
        {
            get
            {
                return holidayList;
            }

            set
            {
                holidayList = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Enumerations.BusinessDayConvention BusinessDayConventionEnum
        {
            get
            {
                return businessDayConvention;
            }

            set
            {
                businessDayConvention = value;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public struct Table_301
        {
            /// <summary>
            /// 
            /// </summary>
            public double EffectiveDate;

            /// <summary>
            /// 
            /// </summary>
            public double TerminatingDate;

            /// <summary>
            /// 
            /// </summary>
            public double Rate;

            /// <summary>
            /// 
            /// </summary>
            public double RateQuotationBasis;

            /// <summary>
            /// 
            /// </summary>
            public double AccrualMethodForOutputRates;

            /// <summary>
            /// 
            /// </summary>
            public double UseThisPoint;
        }

        /// <summary>
        /// 
        /// </summary>
        public struct Table_302
        {
            /// <summary>
            /// 
            /// </summary>
            public double EffectiveDate;

            /// <summary>
            /// 
            /// </summary>
            public double TerminatingDate;

            /// <summary>
            /// 
            /// </summary>
            public double Rate;

            /// <summary>
            /// 
            /// </summary>
            public double RateQuotationBasis;

            /// <summary>
            /// 
            /// </summary>
            public double AccrualMethodForOutputRates;

            /// <summary>
            /// 
            /// </summary>
            public double BusinessDayConvention;

            /// <summary>
            /// 
            /// </summary>
            public double UseThisPoint;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsFincadEnabled()
        {
            bool result = File.Exists("fcvb.dll");
            if (!result)
            {
                string pathString = Environment.GetEnvironmentVariable("PATH");
                string[] paths = pathString.Split(';');
                foreach (string path in paths)
                {
                    result = File.Exists(path + "\\fcvb.dll");
                    if (result)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="basis"></param>
        /// <returns></returns>
        public Enumerations.AccrualMethod ExcelToFincad_AccrualMethod(int? basis)
        {
            Enumerations.AccrualMethod result = Enumerations.AccrualMethod.No_Value;
            if (basis.HasValue)
            {
                switch (basis)
                {
                    case 0:
                        result = Enumerations.AccrualMethod.ISDA_30_360;
                        break;  // 30/360 : 30/360
                    case 1:
                        result = Enumerations.AccrualMethod.Actual_365;
                        break;  // actual/actual : actual/365 (actual)
                    case 2:
                        result = Enumerations.AccrualMethod.Actual_360;
                        break;  // actual/360 : actual/360
                    case 3:
                        result = Enumerations.AccrualMethod.ISMA_Actual_Actual;
                        break;  // actual/365 : actual/365 (fixed)
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compoundingFrequency"></param>
        /// <returns></returns>
        public Enumerations.CompoundingFrequency ExcelToFincad_Frequency(int? compoundingFrequency)
        {
            Enumerations.CompoundingFrequency result = Enumerations.CompoundingFrequency.NoValue;
            if (compoundingFrequency.HasValue)
            {
                switch (compoundingFrequency)
                {
                    case Constants.FREQUENCY_ANNUAL:
                        result = Enumerations.CompoundingFrequency.Annual;
                        break;  // annual
                    case Constants.FREQUENCY_SEMI_ANNUAL:
                        result = Enumerations.CompoundingFrequency.SemiAnnual;
                        break;  // semi-annual
                    case Constants.FREQUENCY_QUARTERLY:
                        result = Enumerations.CompoundingFrequency.Quarterly;
                        break;  // quarterly
                    case Constants.FREQUENCY_MONTHLY:
                        result = Enumerations.CompoundingFrequency.Monthly;
                        break;  // monthly
                    case Constants.FREQUENCY_WEEKLY:
                        result = Enumerations.CompoundingFrequency.Weekly;
                        break;  // monthly
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maturity"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="refDate"></param>
        /// <returns></returns>
        public DateTime GetLastCouponDateGivenDate(DateTime maturity, int compoundingFrequency, DateTime? refDate)
        {
            DateTime result = Convert.ToDateTime(null);

            int months = 0;
            if (compoundingFrequency > 0)
            {
                months = (12 / compoundingFrequency) * -1;
            }

            result = maturity;
            while (refDate.HasValue && result >= refDate)
            {
                try
                {
                    result = result.AddMonths(months);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="maturity"></param>
        /// <param name="compoundingFrequency"></param>
        /// <returns></returns>
        public DateTime GetLastCouponDate(DateTime maturity, int compoundingFrequency)
        {
            DateTime result = Convert.ToDateTime(null);

            int months = 0;
            if (compoundingFrequency > 0)
            {
                months = (12 / compoundingFrequency) * -1;
            }

            try
            {
                result = maturity.AddMonths(months);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return result;
        }

        // FinCAD Functions ****************************************************

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_e"></param>
        /// <param name="d_t"></param>
        /// <param name="acc"></param>
        /// <param name="return_days"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaAccrual_days(
            double d_e,
            double d_t,
            short acc,
            ref double return_days);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effectiveDate"></param>
        /// <param name="terminatingDate"></param>
        /// <param name="accrualMethod"></param>
        /// <returns></returns>
        public int aaAccrual_days(
            DateTime effectiveDate,
            DateTime terminatingDate,
            Enumerations.AccrualMethod accrualMethod)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            int result = 0;
            short fincadResult = -1;
            double d_e = General.ConvertDateToDouble(effectiveDate);
            double d_t = General.ConvertDateToDouble(terminatingDate);
            short acc = (short)accrualMethod;
            double return_days = 0.0;

            try
            {
                fincadResult = aaAccrual_days(
                    d_e,
                    d_t,
                    acc,
                    ref return_days);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                try
                {
                    result = Convert.ToInt32(return_days);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            else if (fincadResult == -1)
            {
                errorMessage = Messages.GeneralError + " (aaAccrual_days)";
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="princ_m"></param>
        /// <param name="coupon"></param>
        /// <param name="freq"></param>
        /// <param name="d_n_cf"></param>
        /// <param name="d_v"></param>
        /// <param name="d_prev_cf"></param>
        /// <param name="acc"></param>
        /// <param name="acc_int_ppar"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaAccrued(
            double princ_m,
            double cpn,
            short freq,
            double d_n_cf,
            double d_v,
            double d_prev_cf,
            short acc,
            ref double acc_int_ppar);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principalAtMaturity"></param>
        /// <param name="coupon"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="nextCashFlowDate"></param>
        /// <param name="settleDate"></param>
        /// <param name="previousCashFlowDate"></param>
        /// <param name="accrualMethod"></param>
        /// <returns></returns>
        public decimal aaAccrued(
            decimal principalAtMaturity,
            decimal coupon,
            Enumerations.CompoundingFrequency compoundingFrequency,
            DateTime nextCashFlowDate,
            DateTime settleDate,
            DateTime previousCashFlowDate,
            Enumerations.AccrualMethod accrualMethod)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            decimal result = 0;
            short fincadResult = -1;
            double princ_m = General.ConvertDecimalToDouble(principalAtMaturity);
            double cpn = General.ConvertDecimalToDouble(coupon);
            short freq = (short)compoundingFrequency;
            double d_n_cf = General.ConvertDateToDouble(nextCashFlowDate);
            double d_v = General.ConvertDateToDouble(settleDate);
            double d_prev_cf = General.ConvertDateToDouble(previousCashFlowDate);
            short acc = (short)accrualMethod;
            double acc_int_ppar = 0.0;

            try
            {
                fincadResult = aaAccrued(
                    princ_m,
                    cpn,
                    freq,
                    d_n_cf,
                    d_v,
                    d_prev_cf,
                    acc,
                    ref acc_int_ppar);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = Convert.ToDecimal(acc_int_ppar);
            }
            else if (fincadResult == -1)
            {
                errorMessage = Messages.GeneralError + " (aaAccrued)";
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cpn"></param>
        /// <param name="d_m"></param>
        /// <param name="d_v"></param>
        /// <param name="price"></param>
        /// <param name="rate_repo"></param>
        /// <param name="d_del"></param>
        /// <param name="d_rul"></param>
        /// <param name="stat"></param>
        /// <param name="c_factor"></param>
        /// <param name="freq"></param>
        /// <param name="acc"></param>
        /// <param name="acc_rate"></param>
        /// <param name="stat_149"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaBFwd2(
            double cpn,
            double d_m,
            double d_v,
            double price,
            double rate_repo,
            double d_del,
            short d_rul,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)2)] ref Array stat,
            double c_factor,
            short freq,
            short acc,
            short acc_rate,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array stat_149);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coupon"></param>
        /// <param name="maturity"></param>
        /// <param name="settleDate"></param>
        /// <param name="tradePrice"></param>
        /// <param name="repoRate"></param>
        /// <param name="forwardDeliveryDate"></param>
        /// <param name="businessDayConvention"></param>
        /// <param name="conversionFactor"></param>
        /// <param name="cashFlowFrequency"></param>
        /// <param name="bondAccrualMethod"></param>
        /// <param name="repoAccrualMethod"></param>
        /// <returns></returns>
        public double[,] aaBFwd2(
            decimal coupon,
            DateTime maturity,
            DateTime settleDate,
            decimal tradePrice,
            decimal repoRate,
            DateTime forwardDeliveryDate,
            Enumerations.BusinessDayConvention businessDayConvention,
            decimal conversionFactor,
            Enumerations.CompoundingFrequency cashFlowFrequency,
            Enumerations.AccrualMethod bondAccrualMethod,
            Enumerations.AccrualMethod repoAccrualMethod)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double cpn = General.ConvertDecimalToDouble(coupon) / 100;
            double d_m = General.ConvertDateToDouble(maturity);
            double d_v = General.ConvertDateToDouble(settleDate);
            double price = General.ConvertDecimalToDouble(tradePrice);
            double rate_repo = General.ConvertDecimalToDouble(repoRate);
            double d_del = General.ConvertDateToDouble(forwardDeliveryDate);
            short d_rul = (short)businessDayConvention; // businessDayConvention
            Array stat = new short[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
            double c_factor = General.ConvertDecimalToDouble(conversionFactor);
            short freq = (short)cashFlowFrequency;
            short acc = (short)bondAccrualMethod;
            short acc_rate = (short)repoAccrualMethod;
            Array stat_149 = new double[1, 1];

            try
            {
                fincadResult = aaBFwd2(
                    cpn,
                    d_m,
                    d_v,
                    price,
                    rate_repo,
                    d_del,
                    d_rul,
                    ref stat,
                    c_factor,
                    freq,
                    acc,
                    acc_rate,
                    ref stat_149);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])stat_149;
            }
            else if (fincadResult == -1)
            {
                errorMessage = Messages.GeneralError + " (BFwd2)";
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="price_u"></param>
        /// <param name="ex"></param>
        /// <param name="d_exp"></param>
        /// <param name="d_v"></param>
        /// <param name="price"></param>
        /// <param name="rate_ann"></param>
        /// <param name="option_type"></param>
        /// <param name="acc"></param>
        /// <param name="vlty_imp"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaBL_iv(
            double price_u,
            double ex,
            double d_exp,
            double d_v,
            double price,
            double rate_ann,
            short option_type,
            short acc,
            ref double vlty_imp);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="underlyingPrice"></param>
        /// <param name="exercisePrice"></param>
        /// <param name="exprationDate"></param>
        /// <param name="settleDate"></param>
        /// <param name="tradePrice"></param>
        /// <param name="annualCompoundingRate"></param>
        /// <param name="optionType"></param>
        /// <param name="accrualMethod"></param>
        /// <returns></returns>
        public decimal aaBL_iv(
            decimal underlyingPrice,
            decimal exercisePrice,
            DateTime exprationDate,
            DateTime settleDate,
            decimal tradePrice,
            decimal annualCompoundingRate,
            int optionType,
            Enumerations.AccrualMethod accrualMethod)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            decimal result = 0;
            short fincadResult = -1;
            double price_u = General.ConvertDecimalToDouble(underlyingPrice);
            double ex = General.ConvertDecimalToDouble(exercisePrice);
            double d_exp = General.ConvertDateToDouble(exprationDate);
            double d_v = General.ConvertDateToDouble(settleDate);
            double price = General.ConvertDecimalToDouble(tradePrice);
            double rate_ann = General.ConvertDecimalToDouble(annualCompoundingRate);
            short option_type = (short)optionType;
            short acc = (short)accrualMethod;
            double vlty_imp = 0.0;

            try
            {
                fincadResult = aaBL_iv(
                    price_u,
                    ex,
                    d_exp,
                    d_v,
                    price,
                    rate_ann,
                    option_type,
                    acc,
                    ref vlty_imp);
            }
            catch (Exception except)
            {
                errorOccurred = true;
                errorMessage = except.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (decimal)vlty_imp;
            }
            else if (fincadResult == -1)
            {
                if (price_u <= 0)
                {
                    errorMessage = Messages.BL_iv_1;
                }
                else if (ex <= 0)
                {
                    errorMessage = Messages.BL_iv_2;
                }
                else if (d_exp <= d_v)
                {
                    errorMessage = Messages.BL_iv_3;
                }
                else if (rate_ann < 0)
                {
                    errorMessage = Messages.BL_iv_4;
                }
                else
                {
                    errorMessage = Messages.GeneralError + " (aaBL_iv)";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_s"></param>
        /// <param name="d_m"></param>
        /// <param name="d_dated"></param>
        /// <param name="d_f_cpn"></param>
        /// <param name="d_l_cpn"></param>
        /// <param name="cpn"></param>
        /// <param name="princ_m"></param>
        /// <param name="freq"></param>
        /// <param name="acc"></param>
        /// <param name="hl"></param>
        /// <param name="d_rul"></param>
        /// <param name="exdays"></param>
        /// <param name="table_type"></param>
        /// <param name="cfx_bond"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaBond_cf(
            double d_s,
            double d_m,
            double d_dated,
            double d_f_cpn,
            double d_l_cpn,
            double cpn,
            double princ_m,
            short freq,
            short acc,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array hl,
            short d_rul,
            double exdays,
            short table_type,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array cfx_bond);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="maturity"></param>
        /// <param name="datedDate"></param>
        /// <param name="firstCouponDate"></param>
        /// <param name="lastCouponDate"></param>
        /// <param name="coupon"></param>
        /// <param name="principalAtMaturity"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="accrualMethod"></param>
        /// <param name="daysExDividend"></param>
        /// <param name="tableType"></param>
        /// <returns></returns>
        public double[,] aaBond_cf(
            DateTime settleDate,
            DateTime maturity,
            DateTime datedDate,
            DateTime firstCouponDate,
            DateTime lastCouponDate,
            decimal coupon,
            decimal principalAtMaturity,
            Enumerations.CompoundingFrequency compoundingFrequency,
            Enumerations.AccrualMethod accrualMethod,
            int daysExDividend,
            int tableType)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double d_s = General.ConvertDateToDouble(settleDate);
            double d_m = General.ConvertDateToDouble(maturity);
            double d_dated = General.ConvertDateToDouble(datedDate);
            double d_f_cpn = General.ConvertDateToDouble(firstCouponDate);
            double d_l_cpn = General.ConvertDateToDouble(lastCouponDate);
            double cpn = General.ConvertDecimalToDouble(coupon);
            double princ_m = General.ConvertDecimalToDouble(principalAtMaturity);
            short freq = (short)compoundingFrequency;
            short acc_bond = (short)accrualMethod;
            Array hl = holidayList;
            short d_rul = (short)businessDayConvention;
            double exdays = daysExDividend;
            short table_type = (short)tableType;
            Array cfx_bond = new double[1, 5];

            if (cpn == 0)
            {
                d_f_cpn = 0;
                d_l_cpn = 0;
            }

            try
            {
                fincadResult = aaBond_cf(
                    d_s,
                    d_m,
                    d_dated,
                    d_f_cpn,
                    d_l_cpn,
                    cpn,
                    princ_m,
                    freq,
                    acc_bond,
                    ref hl,
                    d_rul,
                    exdays,
                    table_type,
                    ref cfx_bond);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])cfx_bond;
            }
            else if (fincadResult == -1)
            {
                if (d_m <= d_s)
                {
                    errorMessage = Messages.Bond_cf_1;
                }
                else if (d_dated > d_s)
                {
                    errorMessage = Messages.Bond_cf_2;
                }
                else if (d_f_cpn <= d_dated)
                {
                    errorMessage = Messages.Bond_cf_3;
                }
                else if (d_l_cpn > d_m)
                {
                    errorMessage = Messages.Bond_cf_4;
                }
                else if (cpn < 0)
                {
                    errorMessage = Messages.Bond_cf_5;
                }
                else if (princ_m < 0)
                {
                    errorMessage = Messages.Bond_cf_6;
                }
                else if (hl.Length <= 1)
                {
                    errorMessage = Messages.Bond_cf_7;
                }
                else
                {
                    errorMessage = Messages.GeneralError + " (aaBond_cf)";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_v"></param>
        /// <param name="cash_crv"></param>
        /// <param name="fut_crv"></param>
        /// <param name="bond_crv"></param>
        /// <param name="method_boot"></param>
        /// <param name="rate_basis"></param>
        /// <param name="acc_rate"></param>
        /// <param name="hl"></param>
        /// <param name="method_gen"></param>
        /// <param name="rated_obj_305"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaBond_crv2(
            double d_v,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array cash_crv,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array fut_crv,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array bond_crv,
            short method_boot,
            short rate_basis,
            short acc_rate,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array hl,
            short method_gen,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array rated_obj_305);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="yieldCurve"></param>
        /// <param name="futuresCurve"></param>
        /// <param name="bondCurve"></param>
        /// <param name="bootstrappingMethod"></param>
        /// <param name="rateQuotationBasis"></param>
        /// <param name="accrualMethod"></param>
        /// <param name="curveGenerationMethod"></param>
        /// <returns></returns>
        public double[,] aaBond_crv2(
            DateTime settleDate,
            double[,] yieldCurve,
            double[,] futuresCurve,
            double[,] bondCurve,
            Enumerations.BootstrappingMethod bootstrappingMethod,
            Enumerations.RateQuotationBasis rateQuotationBasis,
            Enumerations.AccrualMethod accrualMethod,
            Enumerations.CurveGenerationMethod curveGenerationMethod)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            Array cash_crv = yieldCurve;
            Array fut_crv = futuresCurve;
            Array bond_crv = bondCurve;
            double d_v = General.ConvertDateToDouble(settleDate);
            short method_boot = (short)bootstrappingMethod;
            short rate_basis = (short)rateQuotationBasis;
            short acc_rate = (short)accrualMethod;
            Array hl = holidayList;
            short method_gen = (short)curveGenerationMethod;
            Array rated_obj_305 = new double[1, 3] { { 1, 2, 3 } };

            try
            {
                fincadResult = aaBond_crv2(
                    d_v,
                    ref cash_crv,
                    ref fut_crv,
                    ref bond_crv,
                    method_boot,
                    rate_basis,
                    acc_rate,
                    ref hl,
                    method_gen,
                    ref rated_obj_305);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])rated_obj_305;
            }
            else if (fincadResult == -1)
            {
                errorMessage = Messages.GeneralError + " (aaBond_crv2)";
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_s"></param>
        /// <param name="d_m"></param>
        /// <param name="d_dated"></param>
        /// <param name="d_f_cpn"></param>
        /// <param name="cpn"></param>
        /// <param name="princ_m"></param>
        /// <param name="stat"></param>
        /// <param name="stat_251"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaBond_US_accrued(
            double d_s,
            double d_m,
            double d_dated,
            double d_f_cpn,
            double cpn,
            double princ_m,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)2)] ref Array stat,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array stat_251);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="maturity"></param>
        /// <param name="datedDate"></param>
        /// <param name="firstCouponDate"></param>
        /// <param name="coupon"></param>
        /// <param name="principal"></param>
        /// <returns></returns>
        public double[,] aaBond_US_accrued(
            DateTime settleDate,
            DateTime maturity,
            DateTime datedDate,
            DateTime firstCouponDate,
            decimal coupon,
            decimal principal)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double d_s = General.ConvertDateToDouble(settleDate);
            double d_m = General.ConvertDateToDouble(maturity);
            double d_dated = General.ConvertDateToDouble(datedDate);
            double d_f_cpn = General.ConvertDateToDouble(firstCouponDate);
            double cpn = General.ConvertDecimalToDouble(coupon);
            double princ_m = General.ConvertDecimalToDouble(principal);
            Array stat = new short[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
            Array stat_251 = new double[1, 1];

            try
            {
                fincadResult = aaBond_US_accrued(
                    d_s,
                    d_m,
                    d_dated,
                    d_f_cpn,
                    cpn,
                    princ_m,
                    ref stat,
                    ref stat_251);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])stat_251;
            }
            else if (fincadResult == -1)
            {
                errorMessage = Messages.GeneralError + " (aaBond_US_accrued)";
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_s"></param>
        /// <param name="d_m"></param>
        /// <param name="d_dated"></param>
        /// <param name="d_f_cpn"></param>
        /// <param name="d_l_cpn"></param>
        /// <param name="cpn"></param>
        /// <param name="princ_m"></param>
        /// <param name="freq"></param>
        /// <param name="acc"></param>
        /// <param name="exdays"></param>
        /// <param name="stat"></param>
        /// <param name="stat_252"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaBond_accrued(
            double d_s,
            double d_m,
            double d_dated,
            double d_f_cpn,
            double d_l_cpn,
            double cpn,
            double princ_m,
            short freq,
            short acc,
            double exdays,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)2)] ref Array stat,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array stat_252);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="maturity"></param>
        /// <param name="datedDate"></param>
        /// <param name="firstCouponDate"></param>
        /// <param name="lastCouponDate"></param>
        /// <param name="coupon"></param>
        /// <param name="principal"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="accrualMethod"></param>
        /// <param name="daysExDividend"></param>
        /// <returns></returns>
        public double[,] aaBond_accrued(
            DateTime settleDate,
            DateTime maturity,
            DateTime datedDate,
            DateTime firstCouponDate,
            DateTime lastCouponDate,
            decimal coupon,
            decimal principal,
            Enumerations.CompoundingFrequency compoundingFrequency,
            Enumerations.AccrualMethod accrualMethod,
            int daysExDividend)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double d_s = General.ConvertDateToDouble(settleDate);
            double d_m = General.ConvertDateToDouble(maturity);
            double d_dated = General.ConvertDateToDouble(datedDate);
            double d_f_cpn = General.ConvertDateToDouble(firstCouponDate);
            double d_l_cpn = General.ConvertDateToDouble(lastCouponDate);
            double cpn = General.ConvertDecimalToDouble(coupon);
            double princ_m = General.ConvertDecimalToDouble(principal);
            short freq = (short)compoundingFrequency;
            short acc = (short)accrualMethod;
            double exdays = daysExDividend;
            Array stat = new short[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
            Array stat_252 = new double[1, 1];

            if (d_l_cpn == d_f_cpn)
            {
                d_l_cpn = Convert.ToDouble(null);
            }

            try
            {
                fincadResult = aaBond_accrued(
                    d_s,
                    d_m,
                    d_dated,
                    d_f_cpn,
                    d_l_cpn,
                    cpn,
                    princ_m,
                    freq,
                    acc,
                    exdays,
                    ref stat,
                    ref stat_252);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])stat_252;
            }
            else if (fincadResult == -1)
            {
                errorMessage = Messages.GeneralError + " (aaBond_accrued)";
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_s"></param>
        /// <param name="d_m"></param>
        /// <param name="d_dated"></param>
        /// <param name="d_f_cpn"></param>
        /// <param name="d_l_cpn"></param>
        /// <param name="cpn"></param>
        /// <param name="princ_m"></param>
        /// <param name="freq"></param>
        /// <param name="acc"></param>
        /// <param name="yield"></param>
        /// <param name="linlast"></param>
        /// <param name="exdays"></param>
        /// <param name="stat"></param>
        /// <param name="stat_256"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaBond_p(
            double d_s,
            double d_m,
            double d_dated,
            double d_f_cpn,
            double d_l_cpn,
            double cpn,
            double princ_m,
            short freq,
            short acc,
            double yield,
            short linlast,
            double exdays,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)2)] ref Array stat,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array stat_256);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="maturity"></param>
        /// <param name="datedDate"></param>
        /// <param name="firstCouponDate"></param>
        /// <param name="lastCouponDate"></param>
        /// <param name="coupon"></param>
        /// <param name="principalAtMaturity"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="accrualMethod"></param>
        /// <param name="yieldValue"></param>
        /// <param name="rateCompoundingBasis"></param>
        /// <param name="daysExDividend"></param>
        /// <returns></returns>
        public double[,] aaBond_p(
            DateTime settleDate,
            DateTime maturity,
            DateTime datedDate,
            DateTime firstCouponDate,
            DateTime lastCouponDate,
            decimal coupon,
            decimal principalAtMaturity,
            Enumerations.CompoundingFrequency compoundingFrequency,
            Enumerations.AccrualMethod accrualMethod,
            decimal yieldValue,
            Enumerations.RateCompoundingBasis rateCompoundingBasis,
            int daysExDividend)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double d_s = General.ConvertDateToDouble(settleDate);
            double d_m = General.ConvertDateToDouble(maturity);
            double d_dated = General.ConvertDateToDouble(datedDate);
            double d_f_cpn = General.ConvertDateToDouble(firstCouponDate);
            double d_l_cpn = General.ConvertDateToDouble(lastCouponDate);

            if (d_l_cpn >= d_m)
            {
                d_l_cpn = 0;
            }

            double cpn = General.ConvertDecimalToDouble(coupon) / 100;
            double princ_m = General.ConvertDecimalToDouble(principalAtMaturity);
            short freq = (short)compoundingFrequency;
            short acc = (short)accrualMethod;
            double yield = General.ConvertDecimalToDouble(yieldValue);
            short linlast = (short)rateCompoundingBasis;
            double exdays = daysExDividend;
            Array stat = new short[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            Array stat_256 = new double[1, 1];

            if (cpn == 0)
            {
                d_f_cpn = 0;
                d_l_cpn = 0;
            }

            try
            {
                fincadResult = aaBond_p(
                    d_s,
                    d_m,
                    d_dated,
                    d_f_cpn,
                    d_l_cpn,
                    cpn,
                    princ_m,
                    freq,
                    acc,
                    yield,
                    linlast,
                    exdays,
                    ref stat,
                    ref stat_256);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])stat_256;
            }
            else if (fincadResult == -1)
            {
                if (d_m <= d_s)
                {
                    errorMessage = Messages.Bond_p_1;
                }
                else if (d_dated > d_s)
                {
                    errorMessage = Messages.Bond_p_2;
                }
                else if (d_f_cpn <= d_dated)
                {
                    errorMessage = Messages.Bond_p_3;
                }
                else if (d_l_cpn > d_m)
                {
                    errorMessage = Messages.Bond_p_4;
                }
                else if (cpn < 0)
                {
                    errorMessage = Messages.Bond_p_5;
                }
                else if (princ_m < 0)
                {
                    errorMessage = Messages.Bond_p_6;
                }
                else if (yield < 0)
                {
                    errorMessage = Messages.Bond_p_7;
                }
                else
                {
                    errorMessage = Messages.GeneralError + " (aaBond_p)";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_s"></param>
        /// <param name="d_m"></param>
        /// <param name="d_dated"></param>
        /// <param name="d_f_cpn"></param>
        /// <param name="d_l_cpn"></param>
        /// <param name="cpn"></param>
        /// <param name="princ_m"></param>
        /// <param name="freq"></param>
        /// <param name="acc"></param>
        /// <param name="price"></param>
        /// <param name="linlast"></param>
        /// <param name="exdays"></param>
        /// <param name="stat"></param>
        /// <param name="stat_256"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaBond_y(
            double d_s,
            double d_m,
            double d_dated,
            double d_f_cpn,
            double d_l_cpn,
            double cpn,
            double princ_m,
            short freq,
            short acc,
            double price,
            short linlast,
            double exdays,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)2)] ref Array stat,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array stat_256);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="maturity"></param>
        /// <param name="datedDate"></param>
        /// <param name="firstCouponDate"></param>
        /// <param name="lastCouponDate"></param>
        /// <param name="coupon"></param>
        /// <param name="principalAtMaturity"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="accrualMethod"></param>
        /// <param name="tradePrice"></param>
        /// <param name="rateCompoundingBasis"></param>
        /// <param name="daysExDividend"></param>
        /// <returns></returns>
        public double[,] aaBond_y(
            DateTime settleDate,
            DateTime maturity,
            DateTime datedDate,
            DateTime firstCouponDate,
            DateTime lastCouponDate,
            decimal coupon,
            decimal principalAtMaturity,
            Enumerations.CompoundingFrequency compoundingFrequency,
            Enumerations.AccrualMethod accrualMethod,
            decimal tradePrice,
            Enumerations.RateCompoundingBasis rateCompoundingBasis,
            int daysExDividend)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double d_s = General.ConvertDateToDouble(settleDate);
            double d_m = General.ConvertDateToDouble(maturity);
            double d_dated = General.ConvertDateToDouble(datedDate);
            double d_f_cpn = General.ConvertDateToDouble(firstCouponDate);
            double d_l_cpn = General.ConvertDateToDouble(lastCouponDate);

            if (d_l_cpn >= d_m)
            {
                d_l_cpn = 0;
            }

            double cpn = General.ConvertDecimalToDouble(coupon) / 100;
            double princ_m = General.ConvertDecimalToDouble(principalAtMaturity);
            short freq = (short)compoundingFrequency;
            short acc = (short)accrualMethod;
            double price = General.ConvertDecimalToDouble(tradePrice);
            short linlast = (short)rateCompoundingBasis;
            double exdays = daysExDividend;
            Array stat = new short[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            Array stat_256 = new double[1, 1];

            try
            {
                fincadResult = aaBond_y(
                    d_s,
                    d_m,
                    d_dated,
                    d_f_cpn,
                    d_l_cpn,
                    cpn,
                    princ_m,
                    freq,
                    acc,
                    price,
                    linlast,
                    exdays,
                    ref stat,
                    ref stat_256);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])stat_256;
            }
            else if (fincadResult == -1)
            {
                if (d_m <= d_s)
                    errorMessage = Messages.Bond_y_1;
                else if (d_dated > d_s)
                    errorMessage = Messages.Bond_y_2;
                else if (d_f_cpn <= d_dated)
                    errorMessage = Messages.Bond_y_3;
                else if (d_l_cpn > d_m)
                    errorMessage = Messages.Bond_y_4;
                else if (cpn < 0)
                    errorMessage = Messages.Bond_y_5;
                else if (princ_m <= 0)
                    errorMessage = Messages.Bond_y_6;
                else if (price < 0)
                    errorMessage = Messages.Bond_y_7;
                else
                    errorMessage = Messages.GeneralError + " (aaBond_y)";
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="coupon"></param>
        /// <param name="maturity"></param>
        /// <param name="datedDate"></param>
        /// <param name="firstCouponDate"></param>
        /// <param name="lastCouponDate"></param>
        /// <param name="parCallDate"></param>
        /// <param name="premCallDate"></param>
        /// <param name="premCallPx"></param>
        /// <param name="principalAtMaturity"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="daycount"></param>
        /// <param name="price"></param>
        /// <param name="rateCompoundingBasis"></param>
        /// <param name="daysExDividend"></param>
        /// <returns></returns>
        public DateTime aaBond_WorstMaturity(
            DateTime settleDate,
            decimal coupon,
            DateTime maturity,
            DateTime datedDate,
            DateTime firstCouponDate,
            DateTime lastCouponDate,
            DateTime parCallDate,
            DateTime premCallDate,
            decimal premCallPx,
            decimal principalAtMaturity,
            Enumerations.CompoundingFrequency compoundingFrequency,
            Enumerations.AccrualMethod daycount,
            decimal price,
            Enumerations.RateCompoundingBasis rateCompoundingBasis,
            int daysExDividend)
        {
            DateTime result = Constants.NullDate;
            try
            {
                double currentPx = 0, parPx = 0, premPx = 0;
                double[,] fincadResult = null;
                try
                {
                    fincadResult = aaBond_y(
                        settleDate,
                        maturity,
                        datedDate,
                        firstCouponDate,
                        lastCouponDate,
                        coupon,
                        principalAtMaturity,
                        compoundingFrequency,
                        daycount,
                        price,
                        rateCompoundingBasis,
                        daysExDividend);

                    currentPx = fincadResult[0, 0];
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }

                try
                {
                    fincadResult = aaBond_y(
                        settleDate,
                        parCallDate,
                        datedDate,
                        firstCouponDate,
                        lastCouponDate,
                        coupon,
                        principalAtMaturity,
                        compoundingFrequency,
                        daycount,
                        price,
                        rateCompoundingBasis,
                        daysExDividend);

                    parPx = fincadResult[0, 0];
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }

                try
                {
                    fincadResult = aaBond_y(
                        settleDate,
                        premCallDate,
                        datedDate,
                        firstCouponDate,
                        lastCouponDate,
                        coupon,
                        premCallPx,
                        compoundingFrequency,
                        daycount,
                        price,
                        rateCompoundingBasis,
                        daysExDividend);

                    premPx = fincadResult[0, 0];
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }

                double worst = System.Math.Max(currentPx, System.Math.Max(parPx, premPx));
                if (worst == currentPx)
                {
                    result = maturity;
                }

                if (worst == parPx)
                {
                    result = parCallDate;
                }

                result = premCallDate;
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_s"></param>
        /// <param name="d_m"></param>
        /// <param name="d_dated"></param>
        /// <param name="d_f_cpn"></param>
        /// <param name="d_l_cpn"></param>
        /// <param name="cpn"></param>
        /// <param name="freq"></param>
        /// <param name="acc"></param>
        /// <param name="linlast"></param>
        /// <param name="price"></param>
        /// <param name="hl"></param>
        /// <param name="exdays"></param>
        /// <param name="stat"></param>
        /// <param name="stat_257"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaBond_yields(
            double d_s,
            double d_m,
            double d_dated,
            double d_f_cpn,
            double d_l_cpn,
            double cpn,
            short freq,
            short acc,
            short linlast,
            double price,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array hl,
            double exdays,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)2)] ref Array stat,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array stat_257);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="maturity"></param>
        /// <param name="datedDate"></param>
        /// <param name="firstCouponDate"></param>
        /// <param name="lastCouponDate"></param>
        /// <param name="coupon"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="accrualMethod"></param>
        /// <param name="rateCompoundingBasis"></param>
        /// <param name="tradePrice"></param>
        /// <param name="daysExDividend"></param>
        /// <returns></returns>
        public double[,] aaBond_yields(
            DateTime settleDate,
            DateTime maturity,
            DateTime datedDate,
            DateTime firstCouponDate,
            DateTime lastCouponDate,
            decimal coupon,
            Enumerations.CompoundingFrequency compoundingFrequency,
            Enumerations.AccrualMethod accrualMethod,
            int rateCompoundingBasis,
            decimal tradePrice,
            int daysExDividend)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double d_s = General.ConvertDateToDouble(settleDate);
            double d_m = General.ConvertDateToDouble(maturity);
            double d_dated = General.ConvertDateToDouble(datedDate);
            double d_f_cpn = General.ConvertDateToDouble(firstCouponDate);
            double d_l_cpn = General.ConvertDateToDouble(lastCouponDate);
            double cpn = General.ConvertDecimalToDouble(coupon) / 100;
            short freq = (short)compoundingFrequency;
            short acc = (short)accrualMethod;
            short linlast = (short)rateCompoundingBasis;
            double price = General.ConvertDecimalToDouble(tradePrice);
            Array hl = holidayList;
            double exdays = daysExDividend;
            Array stat = new short[11] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            Array stat_257 = new double[1, 1];

            if (cpn == 0)
            {
                d_f_cpn = 0;
            }

            try
            {
                fincadResult = aaBond_yields(
                    d_s,
                    d_m,
                    d_dated,
                    d_f_cpn,
                    d_l_cpn,
                    cpn,
                    freq,
                    acc,
                    linlast,
                    price,
                    ref hl,
                    exdays,
                    ref stat,
                    ref stat_257);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])stat_257;
            }
            else if (fincadResult == -1)
            {
                errorMessage = Messages.GeneralError + " (aaBond_US_yields)";
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_s"></param>
        /// <param name="d_m"></param>
        /// <param name="d_dated"></param>
        /// <param name="d_f_cpn"></param>
        /// <param name="cpn"></param>
        /// <param name="princ_m"></param>
        /// <param name="price"></param>
        /// <param name="stat"></param>
        /// <param name="stat_255"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaBond_CA_y(
            double d_s,
            double d_m,
            double d_dated,
            double d_f_cpn,
            double cpn,
            double princ_m,
            double price,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)2)] ref Array stat,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array stat_255);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="maturity"></param>
        /// <param name="datedDate"></param>
        /// <param name="firstCouponDate"></param>
        /// <param name="coupon"></param>
        /// <param name="principalAtMaturity"></param>
        /// <param name="tradePrice"></param>
        /// <returns></returns>
        public double[,] aaBond_CA_y(
            DateTime settleDate,
            DateTime maturity,
            DateTime datedDate,
            DateTime firstCouponDate,
            decimal coupon,
            decimal principalAtMaturity,
            decimal tradePrice)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double d_s = General.ConvertDateToDouble(settleDate);
            double d_m = General.ConvertDateToDouble(maturity);
            double d_dated = General.ConvertDateToDouble(datedDate);
            double d_f_cpn = General.ConvertDateToDouble(firstCouponDate);
            double cpn = General.ConvertDecimalToDouble(coupon) / 100;
            double princ_m = General.ConvertDecimalToDouble(principalAtMaturity);
            double price = General.ConvertDecimalToDouble(tradePrice);
            Array stat = new short[15] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            Array stat_255 = new double[1, 1];

            if (cpn == 0)
            {
                d_f_cpn = 0;
            }

            try
            {
                fincadResult = aaBond_CA_y(
                    d_s,
                    d_m,
                    d_dated,
                    d_f_cpn,
                    cpn,
                    princ_m,
                    price,
                    ref stat,
                    ref stat_255);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])stat_255;
            }
            else if (fincadResult == -1)
            {
                if (d_m <= d_s)
                {
                    errorMessage = Messages.Bond_CA_y_1;
                }
                else if (d_dated > d_s)
                {
                    errorMessage = Messages.Bond_CA_y_2;
                }
                else if (d_f_cpn <= d_dated)
                {
                    errorMessage = Messages.Bond_CA_y_3;
                }
                else if (cpn < 0)
                {
                    errorMessage = Messages.Bond_CA_y_4;
                }
                else if (princ_m < 0)
                {
                    errorMessage = Messages.Bond_CA_y_5;
                }
                else if (price < 0)
                {
                    errorMessage = Messages.Bond_CA_y_6;
                }
                else
                {
                    errorMessage = Messages.GeneralError + " (aaBond_CA_y)";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_s"></param>
        /// <param name="d_m"></param>
        /// <param name="d_dated"></param>
        /// <param name="d_f_cpn"></param>
        /// <param name="cpn"></param>
        /// <param name="price"></param>
        /// <param name="hl"></param>
        /// <param name="stat"></param>
        /// <param name="stat_257"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaBond_CA_yields(
            double d_s,
            double d_m,
            double d_dated,
            double d_f_cpn,
            double cpn,
            double price,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array hl,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)2)] ref Array stat,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array stat_257);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="maturity"></param>
        /// <param name="datedDate"></param>
        /// <param name="firstCouponDate"></param>
        /// <param name="coupon"></param>
        /// <param name="tradePrice"></param>
        /// <returns></returns>
        public double[,] aaBond_CA_yields(
            DateTime settleDate,
            DateTime maturity,
            DateTime datedDate,
            DateTime firstCouponDate,
            decimal coupon,
            decimal tradePrice)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double d_s = General.ConvertDateToDouble(settleDate);
            double d_m = General.ConvertDateToDouble(maturity);
            double d_dated = General.ConvertDateToDouble(datedDate);
            double d_f_cpn = General.ConvertDateToDouble(firstCouponDate);
            double cpn = General.ConvertDecimalToDouble(coupon) / 100;
            double price = General.ConvertDecimalToDouble(tradePrice);
            Array hl = holidayList;
            Array stat = new short[11] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            Array stat_257 = new double[1, 1];

            if (cpn == 0)
            {
                d_f_cpn = 0;
            }

            try
            {
                fincadResult = aaBond_CA_yields(
                    d_s,
                    d_m,
                    d_dated,
                    d_f_cpn,
                    cpn,
                    price,
                    ref hl,
                    ref stat,
                    ref stat_257);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])stat_257;
            }
            else if (fincadResult == -1)
            {
                if (d_m <= d_s)
                {
                    errorMessage = Messages.Bond_CA_y_1;
                }
                else if (d_dated > d_s)
                {
                    errorMessage = Messages.Bond_CA_y_2;
                }
                else if (d_f_cpn <= d_dated)
                {
                    errorMessage = Messages.Bond_CA_y_3;
                }
                else if (cpn < 0)
                {
                    errorMessage = Messages.Bond_CA_y_4;
                }
                else if (price < 0)
                {
                    errorMessage = Messages.Bond_CA_y_6;
                }
                else
                {
                    errorMessage = Messages.GeneralError + " (aaBond_CA_yields)";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_s"></param>
        /// <param name="d_m"></param>
        /// <param name="d_dated"></param>
        /// <param name="d_f_cpn"></param>
        /// <param name="cpn"></param>
        /// <param name="princ_m"></param>
        /// <param name="yield"></param>
        /// <param name="stat"></param>
        /// <param name="return_stat"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaBond_US_p(
            double d_s,
            double d_m,
            double d_dated,
            double d_f_cpn,
            double cpn,
            double princ_m,
            double yield,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)2)] ref Array stat,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array return_stat);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="maturity"></param>
        /// <param name="datedDate"></param>
        /// <param name="firstCouponDate"></param>
        /// <param name="coupon"></param>
        /// <param name="principalAtMaturity"></param>
        /// <param name="yieldValue"></param>
        /// <returns></returns>
        public double[,] aaBond_US_p(
            DateTime settleDate,
            DateTime maturity,
            DateTime datedDate,
            DateTime firstCouponDate,
            decimal coupon,
            decimal principalAtMaturity,
            decimal yieldValue)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double d_s = General.ConvertDateToDouble(settleDate);
            double d_m = General.ConvertDateToDouble(maturity);
            double d_dated = General.ConvertDateToDouble(datedDate);
            double d_f_cpn = General.ConvertDateToDouble(firstCouponDate);
            double cpn = General.ConvertDecimalToDouble(coupon) / 100;
            double princ_m = General.ConvertDecimalToDouble(principalAtMaturity);
            double yield = General.ConvertDecimalToDouble(yieldValue);
            Array stat = new short[15] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            Array stat_253 = new double[1, 1];

            if (cpn == 0)
            {
                d_f_cpn = 0;
            }

            try
            {
                fincadResult = aaBond_US_p(
                    d_s,
                    d_m,
                    d_dated,
                    d_f_cpn,
                    cpn,
                    princ_m,
                    yield,
                    ref stat,
                    ref stat_253);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])stat_253;
            }
            else if (fincadResult == -1)
            {
                if (d_m <= d_s)
                {
                    errorMessage = Messages.Bond_US_y_1;
                }
                else if (d_dated > d_s)
                {
                    errorMessage = Messages.Bond_US_y_2;
                }
                else if (d_f_cpn <= d_dated)
                {
                    errorMessage = Messages.Bond_US_y_3;
                }
                else if (cpn < 0)
                {
                    errorMessage = Messages.Bond_US_y_4;
                }
                else if (princ_m < 0)
                {
                    errorMessage = Messages.Bond_US_y_5;
                }
                else if (yield < 0)
                {
                    errorMessage = Messages.Bond_US_y_6;
                }
                else
                {
                    errorMessage = Messages.GeneralError + " (aaBond_US_y)";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_s"></param>
        /// <param name="d_m"></param>
        /// <param name="d_dated"></param>
        /// <param name="d_f_cpn"></param>
        /// <param name="cpn"></param>
        /// <param name="princ_m"></param>
        /// <param name="price"></param>
        /// <param name="stat"></param>
        /// <param name="stat_255"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaBond_US_y(
            double d_s,
            double d_m,
            double d_dated,
            double d_f_cpn,
            double cpn,
            double princ_m,
            double price,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)2)] ref Array stat,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array stat_255);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="maturity"></param>
        /// <param name="datedDate"></param>
        /// <param name="firstCouponDate"></param>
        /// <param name="coupon"></param>
        /// <param name="principalAtMaturity"></param>
        /// <param name="tradePrice"></param>
        /// <returns></returns>
        public double[,] aaBond_US_y(
            DateTime settleDate,
            DateTime maturity,
            DateTime datedDate,
            DateTime firstCouponDate,
            decimal coupon,
            decimal principalAtMaturity,
            decimal tradePrice)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double d_s = General.ConvertDateToDouble(settleDate);
            double d_m = General.ConvertDateToDouble(maturity);
            double d_dated = General.ConvertDateToDouble(datedDate);
            double d_f_cpn = General.ConvertDateToDouble(firstCouponDate);
            double cpn = General.ConvertDecimalToDouble(coupon) / 100;
            double princ_m = General.ConvertDecimalToDouble(principalAtMaturity);
            double price = General.ConvertDecimalToDouble(tradePrice);
            Array stat = new short[15] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            Array stat_255 = new double[1, 1];

            if (cpn == 0)
            {
                d_f_cpn = 0;
            }

            try
            {
                fincadResult = aaBond_US_y(
                    d_s,
                    d_m,
                    d_dated,
                    d_f_cpn,
                    cpn,
                    princ_m,
                    price,
                    ref stat,
                    ref stat_255);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])stat_255;
            }
            else if (fincadResult == -1)
            {
                if (d_m <= d_s)
                {
                    errorMessage = Messages.Bond_US_y_1;
                }
                else if (d_dated > d_s)
                {
                    errorMessage = Messages.Bond_US_y_2;
                }
                else if (d_f_cpn <= d_dated)
                {
                    errorMessage = Messages.Bond_US_y_3;
                }
                else if (cpn < 0)
                {
                    errorMessage = Messages.Bond_US_y_4;
                }
                else if (princ_m < 0)
                {
                    errorMessage = Messages.Bond_US_y_5;
                }
                else if (price < 0)
                {
                    errorMessage = Messages.Bond_US_y_6;
                }
                else
                {
                    errorMessage = Messages.GeneralError + " (aaBond_US_y)";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_s"></param>
        /// <param name="d_m"></param>
        /// <param name="d_dated"></param>
        /// <param name="d_f_cpn"></param>
        /// <param name="cpn"></param>
        /// <param name="price"></param>
        /// <param name="hl"></param>
        /// <param name="stat"></param>
        /// <param name="stat_257"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaBond_US_yields(
            double d_s,
            double d_m,
            double d_dated,
            double d_f_cpn,
            double cpn,
            double price,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array hl,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)2)] ref Array stat,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array stat_257);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="maturity"></param>
        /// <param name="datedDate"></param>
        /// <param name="firstCouponDate"></param>
        /// <param name="coupon"></param>
        /// <param name="tradePrice"></param>
        /// <returns></returns>
        public double[,] aaBond_US_yields(
            DateTime settleDate,
            DateTime maturity,
            DateTime datedDate,
            DateTime firstCouponDate,
            decimal coupon,
            decimal tradePrice)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double d_s = General.ConvertDateToDouble(settleDate);
            double d_m = General.ConvertDateToDouble(maturity);
            double d_dated = General.ConvertDateToDouble(datedDate);
            double d_f_cpn = General.ConvertDateToDouble(firstCouponDate);
            double cpn = General.ConvertDecimalToDouble(coupon) / 100;
            double price = General.ConvertDecimalToDouble(tradePrice);
            Array hl = holidayList;
            Array stat = new short[11] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            Array stat_257 = new double[1, 1];

            if (cpn == 0)
            {
                d_f_cpn = 0;
            }

            try
            {
                fincadResult = aaBond_US_yields(
                    d_s,
                    d_m,
                    d_dated,
                    d_f_cpn,
                    cpn,
                    price,
                    ref hl,
                    ref stat,
                    ref stat_257);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])stat_257;
            }
            else if (fincadResult == -1)
            {
                errorMessage = Messages.GeneralError + " (aaBond_US_yields)";
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="df_crv"></param>
        /// <param name="rate_basis"></param>
        /// <param name="acc_rate"></param>
        /// <param name="rate_crv2"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaConvertDF_Rcrv(
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array df_crv,
            short rate_basis,
            short acc_rate,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array rate_crv2);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="discountFactorCurve"></param>
        /// <param name="rateQuotationBasis"></param>
        /// <param name="accrualMethod"></param>
        /// <returns></returns>
        public double[,] aaConvertDF_Rcrv(
            double[,] discountFactorCurve,
            Enumerations.RateQuotationBasis rateQuotationBasis,
            Enumerations.AccrualMethod accrualMethod)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            Array df_crv = discountFactorCurve;
            short rate_basis = (short)rateQuotationBasis;
            short acc = (short)accrualMethod;
            Array rate_crv2 = new double[0, 0];

            try
            {
                fincadResult = aaConvertDF_Rcrv(
                    ref df_crv,
                    rate_basis,
                    acc,
                    ref rate_crv2);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])rate_crv2;
            }
            else if (fincadResult == -1)
            {
                errorMessage = Messages.GeneralError + " (aaConvertDF_Rcrv)";
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_to_adj"></param>
        /// <param name="num_units"></param>
        /// <param name="adj_units"></param>
        /// <param name="d_rul"></param>
        /// <param name="stat_hl"></param>
        /// <param name="d_adj_wh"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaDateAdjust(
            double d_to_adj,
            double num_units,
            short adj_units,
            short d_rul,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array stat_hl,
            ref double d_adj_wh);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateToAdjust"></param>
        /// <param name="numberOfUnitsToAdjust"></param>
        /// <param name="dateAdjustmentMethod"></param>
        /// <returns></returns>
        public DateTime aaDateAdjust(
            DateTime dateToAdjust,
            decimal numberOfUnitsToAdjust,
            Enumerations.DateAdjustmentMethod dateAdjustmentMethod)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            DateTime result = Convert.ToDateTime(null);
            short fincadResult = -1;
            double d_to_adjust = General.ConvertDateToDouble(dateToAdjust);
            double num_units = General.ConvertDecimalToDouble(numberOfUnitsToAdjust);
            short adj_units = (short)dateAdjustmentMethod;
            short d_rul = (short)businessDayConvention;
            Array hl = holidayList;
            double d_adj_wh = 0.0;

            try
            {
                fincadResult = aaDateAdjust(
                    d_to_adjust,
                    num_units,
                    adj_units,
                    d_rul,
                    ref hl,
                    ref d_adj_wh);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = DateTime.FromOADate(d_adj_wh);
            }
            else if (fincadResult == -1)
            {
                errorMessage = Messages.GeneralError + " (aaConvertDF_Rcrv)";
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_s"></param>
        /// <param name="d_t"></param>
        /// <param name="d_e"></param>
        /// <param name="d_f_cpn"></param>
        /// <param name="d_l_cpn"></param>
        /// <param name="cpn"></param>
        /// <param name="princ"></param>
        /// <param name="freq"></param>
        /// <param name="acc"></param>
        /// <param name="d_rul"></param>
        /// <param name="hl"></param>
        /// <param name="stat"></param>
        /// <param name="stat_251"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaFixlg_accrued(
            double d_s,
            double d_t,
            double d_e,
            double d_f_cpn,
            double d_l_cpn,
            double cpn,
            double princ,
            short freq,
            short acc,
            short d_rul,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array hl,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)2)] ref Array stat,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array stat_251);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="maturity"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="firstCouponDate"></param>
        /// <param name="lastCouponDate"></param>
        /// <param name="coupon"></param>
        /// <param name="principal"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="accrualMethod"></param>
        /// <returns></returns>
        public double[,] aaFixlg_accrued(
            DateTime settleDate,
            DateTime maturity,
            DateTime effectiveDate,
            DateTime firstCouponDate,
            DateTime lastCouponDate,
            decimal coupon,
            decimal principal,
            Enumerations.CompoundingFrequency compoundingFrequency,
            Enumerations.AccrualMethod accrualMethod)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double d_s = General.ConvertDateToDouble(settleDate);
            double d_t = General.ConvertDateToDouble(maturity);
            double d_e = General.ConvertDateToDouble(effectiveDate);
            double d_f_cpn = General.ConvertDateToDouble(firstCouponDate);
            double d_l_cpn = General.ConvertDateToDouble(lastCouponDate);
            double cpn = General.ConvertDecimalToDouble(coupon);
            double princ = General.ConvertDecimalToDouble(principal);
            short freq = (short)compoundingFrequency;
            short acc = (short)accrualMethod;
            short d_rul = (short)businessDayConvention;
            Array hl = holidayList;
            Array stat = new short[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
            Array stat_251 = new double[1, 1];

            if (d_l_cpn == d_f_cpn)
            {
                d_l_cpn = Convert.ToDouble(null);
            }

            try
            {
                fincadResult = aaFixlg_accrued(
                    d_s,
                    d_t,
                    d_e,
                    d_f_cpn,
                    d_l_cpn,
                    cpn,
                    princ,
                    freq,
                    acc,
                    d_rul,
                    ref hl,
                    ref stat,
                    ref stat_251);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])stat_251;
            }
            else if (fincadResult == -1)
            {
                if (d_e >= d_t)
                {
                    errorMessage = Messages.Fixlg_accrued_1;
                }
                else if (d_f_cpn <= d_e)
                {
                    errorMessage = Messages.Fixlg_accrued_2;
                }
                else if (d_l_cpn > d_t)
                {
                    errorMessage = Messages.Fixlg_accrued_3;
                }
                else if (cpn < 0)
                {
                    errorMessage = Messages.Fixlg_accrued_4;
                }
                else if (princ < 0)
                {
                    errorMessage = Messages.Fixlg_accrued_5;
                }
                else if (hl.Length <= 1)
                {
                    errorMessage = Messages.Fixlg_accrued_6;
                }
                else
                {
                    errorMessage = Messages.GeneralError + " (aaFixlg_accrued)";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_v"></param>
        /// <param name="d_e"></param>
        /// <param name="d_t"></param>
        /// <param name="cpn"></param>
        /// <param name="freq"></param>
        /// <param name="odd"></param>
        /// <param name="d_rul"></param>
        /// <param name="d_dir"></param>
        /// <param name="acc"></param>
        /// <param name="hl"></param>
        /// <param name="npa"></param>
        /// <param name="asset"></param>
        /// <param name="cf_table"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaFixleg_cf(
            double d_v,
            double d_e,
            double d_t,
            double cpn,
            short freq,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array odd,
            short d_rul,
            short d_dir,
            short acc,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array hl,
            double npa,
            short asset,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array cf_table);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tradeDate"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="maturity"></param>
        /// <param name="coupon"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="dateGenerationDirection"></param>
        /// <param name="accrualMethod"></param>
        /// <param name="notional"></param>
        /// <param name="exchangeOfPrincipal"></param>
        /// <returns></returns>
        public double[,] aaFixleg_cf(
            DateTime tradeDate,
            DateTime effectiveDate,
            DateTime maturity,
            decimal coupon,
            Enumerations.CompoundingFrequency compoundingFrequency,
            Enumerations.DateGenerationDirection dateGenerationDirection,
            Enumerations.AccrualMethod accrualMethod,
            decimal notional,
            Enumerations.ExchangeOfPrincipal exchangeOfPrincipal)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double[,] return_stat = new double[1, 1];
            double d_v = General.ConvertDateToDouble(tradeDate);
            double d_e = General.ConvertDateToDouble(effectiveDate);
            double d_t = General.ConvertDateToDouble(maturity);
            double cpn = General.ConvertDecimalToDouble(coupon);
            short freq = (short)compoundingFrequency;
            Array odd = new double[1] { 0 };
            short d_rul = (short)businessDayConvention;
            short d_dir = (short)dateGenerationDirection;
            short acc = (short)accrualMethod;
            Array hl = holidayList;
            double npa = General.ConvertDecimalToDouble(notional);
            short asset = (short)exchangeOfPrincipal;
            Array cf_table = new double[1, 1];

            try
            {
                fincadResult = aaFixleg_cf(
                    d_v,
                    d_e,
                    d_t,
                    cpn,
                    freq,
                    ref odd,
                    d_rul,
                    d_dir,
                    acc,
                    ref hl,
                    npa,
                    asset,
                    ref cf_table);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])cf_table;
            }
            else if (fincadResult == -1)
            {
                if (d_t <= d_v)
                {
                    errorMessage = Messages.Fixlg_cfx2_3;
                }
                // else if( d_t < 0 )
                //      errorMessage = csMessages.FFINCAD_Fixlg_cfx2_4;
                else if (hl.Length <= 1)
                {
                    errorMessage = Messages.Fixlg_cfx2_6;
                }
                else
                {
                    errorMessage = Messages.GeneralError + " (aaFixleg_cf)";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_v"></param>
        /// <param name="d_e"></param>
        /// <param name="d_t"></param>
        /// <param name="cpn"></param>
        /// <param name="freq"></param>
        /// <param name="odd"></param>
        /// <param name="d_rul"></param>
        /// <param name="d_dir"></param>
        /// <param name="acc"></param>
        /// <param name="intrp"></param>
        /// <param name="hl"></param>
        /// <param name="npa"></param>
        /// <param name="df_crv"></param>
        /// <param name="asset"></param>
        /// <param name="cfx_fix_297"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaFixlg_cfx2(
            double d_v,
            double d_e,
            double d_t,
            double cpn,
            short freq,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array odd,
            short d_rul,
            short d_dir,
            short acc,
            short intrp,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array hl,
            double npa,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array df_crv,
            short asset,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array cfx_fix_297);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tradeDate"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="maturity"></param>
        /// <param name="coupon"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="dateGenerationDirection"></param>
        /// <param name="accrualMethod"></param>
        /// <param name="interpolationMethod"></param>
        /// <param name="notional"></param>
        /// <param name="exchangeOfPrincipal"></param>
        /// <param name="rateCurve"></param>
        /// <returns></returns>
        public double[,] aaFixlg_cfx2(
            DateTime tradeDate,
            DateTime effectiveDate,
            DateTime maturity,
            decimal coupon,
            Enumerations.CompoundingFrequency compoundingFrequency,
            Enumerations.DateGenerationDirection dateGenerationDirection,
            Enumerations.AccrualMethod accrualMethod,
            Enumerations.InterpolationMethod interpolationMethod,
            decimal notional,
            Enumerations.ExchangeOfPrincipal exchangeOfPrincipal,
            double[,] rateCurve)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double d_v = General.ConvertDateToDouble(tradeDate);
            double d_e = General.ConvertDateToDouble(effectiveDate);
            double d_t = General.ConvertDateToDouble(maturity);
            double cpn = General.ConvertDecimalToDouble(coupon);
            short freq = (short)compoundingFrequency;
            Array odd = new double[1] { 0 };
            short d_rul = (short)businessDayConvention;
            short d_dir = (short)dateGenerationDirection;
            short acc = (short)accrualMethod;
            short intrp = (short)interpolationMethod;
            Array hl = holidayList;
            double npa = General.ConvertDecimalToDouble(notional);
            short asset = (short)exchangeOfPrincipal;
            Array df_crv = rateCurve;
            Array cfx_fix_297 = new double[1, 1];

            try
            {
                fincadResult = aaFixlg_cfx2(
                    d_v,
                    d_e,
                    d_t,
                    cpn,
                    freq,
                    ref odd,
                    d_rul,
                    d_dir,
                    acc,
                    intrp,
                    ref hl,
                    npa,
                    ref df_crv,
                    asset,
                    ref cfx_fix_297);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])cfx_fix_297;
            }
            else if (fincadResult == -1)
            {
                if (d_t <= d_v)
                {
                    errorMessage = Messages.Fixlg_cfx2_3;
                }
                else if (rateCurve == null || rateCurve.Length == 0)
                {
                    errorMessage = Messages.Fixlg_cfx2_5;
                }
                else if (hl.Length <= 1)
                {
                    errorMessage = Messages.Fixlg_cfx2_6;
                }
                else
                {
                    errorMessage = Messages.GeneralError + " (aaFixlg_cfx)";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_s"></param>
        /// <param name="d_t"></param>
        /// <param name="d_e"></param>
        /// <param name="d_f_cpn"></param>
        /// <param name="d_l_cpn"></param>
        /// <param name="cpn"></param>
        /// <param name="princ"></param>
        /// <param name="freq"></param>
        /// <param name="acc_rate"></param>
        /// <param name="acc_accrued"></param>
        /// <param name="hl"></param>
        /// <param name="d_rul"></param>
        /// <param name="asset"></param>
        /// <param name="position"></param>
        /// <param name="interp"></param>
        /// <param name="df_crv"></param>
        /// <param name="stat"></param>
        /// <param name="stat_300"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaFixlg_p(
            double d_s,
            double d_t,
            double d_e,
            double d_f_cpn,
            double d_l_cpn,
            double cpn,
            double princ,
            short freq,
            short acc_rate,
            short acc_accrued,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array hl,
            short d_rul,
            short asset,
            short position,
            short interp,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array df_crv,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)2)] ref Array stat,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array stat_300);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="maturity"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="firstCouponDate"></param>
        /// <param name="lastCouponDate"></param>
        /// <param name="coupon"></param>
        /// <param name="principal"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="dayCountOutputRates"></param>
        /// <param name="dayCountAccruedInterest"></param>
        /// <param name="exchangeOfPrincipal"></param>
        /// <param name="tradePosition"></param>
        /// <param name="interpolationMethod"></param>
        /// <param name="rateCurve"></param>
        /// <returns></returns>
        public double[,] aaFixlg_p(
            DateTime settleDate,
            DateTime maturity,
            DateTime effectiveDate,
            DateTime firstCouponDate,
            DateTime lastCouponDate,
            decimal coupon,
            decimal principal,
            Enumerations.CompoundingFrequency compoundingFrequency,
            Enumerations.AccrualMethod dayCountOutputRates,
            Enumerations.AccrualMethod dayCountAccruedInterest,
            Enumerations.ExchangeOfPrincipal exchangeOfPrincipal,
            Enumerations.TradePosition tradePosition,
            Enumerations.InterpolationMethod interpolationMethod,
            double[,] rateCurve)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double d_s = General.ConvertDateToDouble(settleDate);
            double d_t = General.ConvertDateToDouble(maturity);
            double d_e = General.ConvertDateToDouble(effectiveDate);
            double d_f_cpn = General.ConvertDateToDouble(firstCouponDate);
            double d_l_cpn = General.ConvertDateToDouble(lastCouponDate);
            double cpn = General.ConvertDecimalToDouble(coupon);
            double princ = General.ConvertDecimalToDouble(principal);
            short freq = (short)compoundingFrequency;
            short acc_rate = (short)dayCountOutputRates;
            short acc_accrued = (short)dayCountAccruedInterest;
            Array hl = holidayList;
            short d_rul = (short)businessDayConvention;
            short asset = (short)exchangeOfPrincipal;
            short position = (short)tradePosition;
            short intrp = (short)interpolationMethod;
            Array df_crv = rateCurve;
            Array stat = new short[14] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            Array stat_300 = new double[1, 1];

            if (rateCurve.Length > 0 && d_s < rateCurve[0, 0])
            {
                d_s = rateCurve[0, 0];
            }

            if (rateCurve.Length > 0 && d_t < rateCurve[0, 0])
            {
                d_t = rateCurve[0, 0];
            }

            if (d_l_cpn == d_f_cpn)
            {
                d_l_cpn = Convert.ToDouble(null);
            }

            try
            {
                fincadResult = aaFixlg_p(
                    d_s,
                    d_t,
                    d_e,
                    d_f_cpn,
                    d_l_cpn,
                    cpn,
                    princ,
                    freq,
                    acc_rate,
                    acc_accrued,
                    ref hl,
                    d_rul,
                    asset,
                    position,
                    intrp,
                    ref df_crv,
                    ref stat,
                    ref stat_300);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])stat_300;
            }
            else if (fincadResult == -1)
            {
                if (d_e >= d_t)
                {
                    errorMessage = Messages.Fixlg_p_1;
                }
                else if (d_f_cpn <= d_e)
                {
                    errorMessage = Messages.Fixlg_p_2;
                }
                else if (d_l_cpn > d_t)
                {
                    errorMessage = Messages.Fixlg_p_3;
                }
                else if (cpn < 0)
                {
                    errorMessage = Messages.Fixlg_p_4;
                }
                else if (princ < 0)
                {
                    errorMessage = Messages.Fixlg_p_5;
                }
                else if (rateCurve == null || rateCurve.Length == 0)
                {
                    errorMessage = Messages.Fixlg_p_6;
                }
                else if (hl.Length <= 1)
                {
                    errorMessage = Messages.Fixlg_p_7;
                }
                else
                {
                    errorMessage = Messages.GeneralError + " (aaFixlg_p)";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_v"></param>
        /// <param name="d_e"></param>
        /// <param name="d_t"></param>
        /// <param name="freq"></param>
        /// <param name="odd"></param>
        /// <param name="d_rul"></param>
        /// <param name="d_dir"></param>
        /// <param name="acc"></param>
        /// <param name="interp"></param>
        /// <param name="hl"></param>
        /// <param name="npa"></param>
        /// <param name="df_crv"></param>
        /// <param name="asset"></param>
        /// <param name="mgn"></param>
        /// <param name="rate_reset"></param>
        /// <param name="cfx_fl2"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaFlleg_cfx2(
            double d_v,
            double d_e,
            double d_t,
            short freq,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array odd,
            short d_rul,
            short d_dir,
            short acc,
            short interp,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array hl,
            double npa,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array df_crv,
            short asset,
            double mgn,
            double rate_reset,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array cfx_fl2);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tradeDate"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="maturity"></param>
        /// <param name="Margin"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="dateGenerationDirection"></param>
        /// <param name="accrualMethod"></param>
        /// <param name="interpolationMethod"></param>
        /// <param name="notional"></param>
        /// <param name="exchangeOfPrincipal"></param>
        /// <param name="resetRate"></param>
        /// <param name="rateCurve"></param>
        /// <returns></returns>
        public double[,] aaFlleg_cfx2(
            DateTime tradeDate,
            DateTime effectiveDate,
            DateTime maturity,
            decimal Margin,
            Enumerations.CompoundingFrequency compoundingFrequency,
            Enumerations.DateGenerationDirection dateGenerationDirection,
            Enumerations.AccrualMethod accrualMethod,
            Enumerations.InterpolationMethod interpolationMethod,
            decimal notional,
            Enumerations.ExchangeOfPrincipal exchangeOfPrincipal,
            decimal resetRate,
            double[,] rateCurve)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double d_v = General.ConvertDateToDouble(tradeDate);
            double d_e = General.ConvertDateToDouble(effectiveDate);
            double d_t = General.ConvertDateToDouble(maturity);
            short freq = (short)compoundingFrequency;
            Array odd = new double[1] { 0 };
            short d_rul = (short)businessDayConvention;
            short d_dir = (short)dateGenerationDirection;
            short acc = (short)accrualMethod;
            short intrp = (short)interpolationMethod;
            Array hl = holidayList;
            double npa = General.ConvertDecimalToDouble(notional);
            Array df_crv = rateCurve;
            short asset = (short)exchangeOfPrincipal;
            double mgn = General.ConvertDecimalToDouble(Margin);
            double rate_reset = General.ConvertDecimalToDouble(resetRate);
            Array cfx_fl2 = new double[1, 1];

            try
            {
                fincadResult = aaFlleg_cfx2(
                    d_v,
                    d_e,
                    d_t,
                    freq,
                    ref odd,
                    d_rul,
                    d_dir,
                    acc,
                    intrp,
                    ref hl,
                    npa,
                    ref df_crv,
                    asset,
                    mgn,
                    rate_reset,
                    ref cfx_fl2);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])cfx_fl2;
            }
            else if (fincadResult == -1)
            {
                if (d_t <= d_v)
                {
                    errorMessage = Messages.Fixlg_cfx2_3;
                    // else if( d_t < 0 )
                    //      errorMessage = csMessages.FFINCAD_Fixlg_cfx2_4;
                }
                else if (rateCurve == null || rateCurve.Length == 0)
                {
                    errorMessage = Messages.Fixlg_cfx2_5;
                }
                else if (hl.Length <= 1)
                {
                    errorMessage = Messages.Fixlg_cfx2_6;
                }
                else
                {
                    errorMessage = Messages.GeneralError + " (aaFixlg_p)";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x_list"></param>
        /// <param name="xy_list"></param>
        /// <param name="intrp"></param>
        /// <param name="return_y_list"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaInterp(
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array x_list,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array xy_list,
            short intrp,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array return_y_list);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xList"></param>
        /// <param name="xyList"></param>
        /// <param name="interpolationMethod"></param>
        /// <returns></returns>
        public double[,] aaInterp(
            double[] xList,
            double[,] xyList,
            Enumerations.InterpolationMethod interpolationMethod)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double[,] y_list = new double[0, 0];
            double[] x_list = xList;
            double[,] xy_list = xyList;
            short intrp = (short)interpolationMethod;
            Array x_list_arr = x_list;
            Array xy_list_arr = xy_list;
            Array return_y_list_arr = y_list;

            try
            {
                fincadResult = aaInterp(
                    ref x_list_arr,
                    ref xy_list_arr,
                    intrp,
                    ref return_y_list_arr);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])return_y_list_arr;
            }
            else if (fincadResult == -1)
            {
                if (xList == null || xList.Length == 0)
                {
                    errorMessage = Messages.Interp_1;
                }
                else if (xyList == null || xyList.Length == 0)
                {
                    errorMessage = Messages.Interp_2;
                }
                else if (xList[0] > xyList[0, 0])
                {
                    errorMessage = "Requested points are less than provided points.";
                }
                else if (xList[xList.Length - 1] > xyList[xyList.Length / 2 - 1, 0])
                {
                    errorMessage = "Requested points are greater than provided points.";
                }
                else
                {
                    errorMessage = Messages.GeneralError + " (aaInterp)";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_to_adj"></param>
        /// <param name="jump_by"></param>
        /// <param name="jump_fwd"></param>
        /// <param name="d_rul"></param>
        /// <param name="hl"></param>
        /// <param name="d_adj_bd"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaMaturity_date2(
            double d_to_adj,
            short jump_by,
            short jump_fwd,
            short d_rul,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array hl,
            ref double d_adj_bd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateToAdjust"></param>
        /// <param name="jumpBy"></param>
        /// <param name="jumpForward"></param>
        /// <returns></returns>
        public DateTime aaMaturity_date2(
            DateTime dateToAdjust,
            int jumpBy,
            int jumpForward)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            DateTime result = Convert.ToDateTime(null);
            short fincadResult = -1;
            double[,] return_stat = new double[1, 1];
            double dbl_to_adj = General.ConvertDateToDouble(dateToAdjust);
            short short_jump_by = (short)jumpBy;
            short short_jump_fwd = (short)jumpForward;
            short d_rul = (short)businessDayConvention;
            double[] hl = holidayList;
            Array stat_hl = hl;
            double return_d_adj = 0;

            try
            {
                fincadResult = aaMaturity_date2(
                    dbl_to_adj,
                    short_jump_by,
                    short_jump_fwd,
                    d_rul,
                    ref stat_hl,
                    ref return_d_adj);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = DateTime.FromOADate(return_d_adj);
            }
            else if (fincadResult == -1)
            {
                errorMessage = Messages.GeneralError + " (aaMaturity_date2)";
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_s"></param>
        /// <param name="d_t"></param>
        /// <param name="d_e"></param>
        /// <param name="d_f_cpn"></param>
        /// <param name="d_l_cpn"></param>
        /// <param name="freq"></param>
        /// <param name="acc_rate"></param>
        /// <param name="acc_accrued"></param>
        /// <param name="hl"></param>
        /// <param name="d_rul"></param>
        /// <param name="interp"></param>
        /// <param name="df_crv"></param>
        /// <param name="rate_par_swap"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaParSwap(
            double d_s,
            double d_t,
            double d_e,
            double d_f_cpn,
            double d_l_cpn,
            short freq,
            short acc_rate,
            short acc_accrued,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array hl,
            short d_rul,
            short interp,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array df_crv,
            ref double rate_par_swap);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="maturity"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="firstCouponDate"></param>
        /// <param name="lastCouponDate"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="dayCountOutputRates"></param>
        /// <param name="dayCountAccruedInterest"></param>
        /// <param name="interpolationMethod"></param>
        /// <param name="rateCurve"></param>
        /// <returns></returns>
        public decimal aaParSwap(
            DateTime settleDate,
            DateTime maturity,
            DateTime effectiveDate,
            DateTime firstCouponDate,
            DateTime lastCouponDate,
            Enumerations.CompoundingFrequency compoundingFrequency,
            Enumerations.AccrualMethod dayCountOutputRates,
            Enumerations.AccrualMethod dayCountAccruedInterest,
            Enumerations.InterpolationMethod interpolationMethod,
            double[,] rateCurve)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            if (rateCurve[0, 0] > settleDate.ToOADate())
            {
                rateCurve[0, 0] = settleDate.ToOADate();
            }

            decimal result = 0;
            short fincadResult = -1;
            double d_s = General.ConvertDateToDouble(settleDate);
            double d_t = General.ConvertDateToDouble(maturity);
            double d_e = General.ConvertDateToDouble(effectiveDate);
            double d_f_cpn = General.ConvertDateToDouble(firstCouponDate);
            double d_l_cpn = General.ConvertDateToDouble(lastCouponDate);
            short freq = (short)compoundingFrequency;
            short acc_rate = (short)dayCountOutputRates;
            short acc_accrued = (short)dayCountAccruedInterest;
            short d_rul = (short)businessDayConvention;
            short intrp = (short)interpolationMethod;
            Array hl = holidayList;
            Array df_crv = rateCurve;
            double rate_par_swap = 0.0;

            if (d_l_cpn == d_f_cpn)
            {
                d_l_cpn = Convert.ToDouble(null);
            }

            try
            {

                fincadResult = aaParSwap(
                    d_s,
                    d_t,
                    d_e,
                    d_f_cpn,
                    d_l_cpn,
                    freq,
                    acc_rate,
                    acc_accrued,
                    ref hl,
                    d_rul,
                    intrp,
                    ref df_crv,
                    ref rate_par_swap);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (decimal)rate_par_swap;
            }
            else if (fincadResult == -1)
            {
                if (d_e <= d_t)
                {
                    errorMessage = Messages.ParSwap_1;
                }
                else if (d_f_cpn <= d_e)
                {
                    errorMessage = Messages.ParSwap_2;
                }
                else if (d_l_cpn > d_t)
                {
                    errorMessage = Messages.ParSwap_3;
                }
                else if (rateCurve == null || rateCurve.Length == 0)
                {
                    errorMessage = Messages.ParSwap_4;
                }
                else if (hl.Length <= 1)
                {
                    errorMessage = Messages.ParSwap_5;
                }
                else
                {
                    errorMessage = Messages.GeneralError + " (aaParSwap)";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_v"></param>
        /// <param name="cash_crv"></param>
        /// <param name="fut_crv"></param>
        /// <param name="swap_crv"></param>
        /// <param name="method_boot"></param>
        /// <param name="rate_basis"></param>
        /// <param name="acc_rate"></param>
        /// <param name="hl"></param>
        /// <param name="method_gen"></param>
        /// <param name="ratedf_obj_305"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaSwap_crv(
            double d_v,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array cash_crv,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array fut_crv,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array swap_crv,
            short method_boot,
            short rate_basis,
            short acc_rate,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array hl,
            short method_gen,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array ratedf_obj_305);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="yieldCurve"></param>
        /// <param name="futuresCurve"></param>
        /// <param name="swapCurve"></param>
        /// <param name="bootstrappingMethod"></param>
        /// <param name="rateQuotationBasis"></param>
        /// <param name="accrualMethod"></param>
        /// <param name="curveGenerationMethod"></param>
        /// <returns></returns>
        public double[,] aaSwap_crv(
            DateTime settleDate,
            double[,] yieldCurve,
            double[,] futuresCurve,
            double[,] swapCurve,
            Enumerations.BootstrappingMethod bootstrappingMethod,
            Enumerations.RateQuotationBasis rateQuotationBasis,
            Enumerations.AccrualMethod accrualMethod,
            Enumerations.CurveGenerationMethod curveGenerationMethod)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double d_v = General.ConvertDateToDouble(settleDate);
            Array cash_crv = yieldCurve;
            Array fut_crv = futuresCurve;
            Array swp_crv = swapCurve;
            short method_boot = (short)bootstrappingMethod;
            short rate_basis = (short)rateQuotationBasis;
            short acc_rate = (short)accrualMethod;
            Array hl = holidayList;
            short method_gen = (short)curveGenerationMethod;
            Array ratedf_obj_305 = new double[1, 3] { { 1, 2, 3 } };

            try
            {
                fincadResult = aaSwap_crv(
                    d_v,
                    ref cash_crv,
                    ref fut_crv,
                    ref swp_crv,
                    method_boot,
                    rate_basis,
                    acc_rate,
                    ref hl,
                    method_gen,
                    ref ratedf_obj_305);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])ratedf_obj_305;
            }
            else if (fincadResult == -1)
            {
                errorMessage = Messages.GeneralError + " (aaSwap_crv)";
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="princ_m"></param>
        /// <param name="d_m"></param>
        /// <param name="d_v"></param>
        /// <param name="yield"></param>
        /// <param name="cmpd_freq"></param>
        /// <param name="d_rul"></param>
        /// <param name="stat"></param>
        /// <param name="hl"></param>
        /// <param name="acc"></param>
        /// <param name="stat_147"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaZCB(
            double princ_m,
            double d_m,
            double d_v,
            double yield,
            short cmpd_freq,
            short d_rul,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)2)] ref Array stat,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array hl,
            short acc,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array stat_147);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="maturity"></param>
        /// <param name="settleDate"></param>
        /// <param name="yieldValue"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="accrualMethod"></param>
        /// <returns></returns>
        public double[,] aaZCB(
            decimal principal,
            DateTime maturity,
            DateTime settleDate,
            decimal yieldValue,
            Enumerations.CompoundingFrequency compoundingFrequency,
            Enumerations.AccrualMethod accrualMethod)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double[,] result = null;
            short fincadResult = -1;
            double princ_m = General.ConvertDecimalToDouble(principal);
            double d_m = General.ConvertDateToDouble(maturity);
            double d_v = General.ConvertDateToDouble(settleDate);
            double yield = General.ConvertDecimalToDouble(yieldValue);
            short cmpd_freq = (short)compoundingFrequency;
            short d_rul = (short)businessDayConvention;
            Array stat = new short[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Array hl = holidayList;
            short acc = (short)accrualMethod;
            Array stat_147 = new double[1, 1];

            try
            {
                fincadResult = aaZCB(
                    princ_m,
                    d_m,
                    d_v,
                    yield,
                    cmpd_freq,
                    d_rul,
                    ref stat,
                    ref hl,
                    acc,
                    ref stat_147);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {
                result = (double[,])stat_147;
            }
            else if (fincadResult == -1)
            {
                if (princ_m < 0)
                {
                    errorMessage = Messages.Zcb_1;
                }
                else if (d_m <= d_v)
                {
                    errorMessage = Messages.Zcb_2;
                }
                else if (yield <= 0)
                {
                    errorMessage = Messages.Zcb_3;
                }
                else if (hl.Length <= 1)
                {
                    errorMessage = Messages.Zcb_4;
                }
                else
                {
                    errorMessage = Messages.GeneralError + " (aaZCB)";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="princ_m"></param>
        /// <param name="d_m"></param>
        /// <param name="d_v"></param>
        /// <param name="price"></param>
        /// <param name="cmpd_freq"></param>
        /// <param name="d_rul"></param>
        /// <param name="acc"></param>
        /// <param name="hl"></param>
        /// <param name="trunc"></param>
        /// <param name="d_yield"></param>
        /// <returns></returns>
        [DllImport("fcvb.dll")]
        static extern short aaZCB_y(
            double princ_m,
            double d_m,
            double d_v,
            double price,
            short cmpd_freq,
            short d_rul,
            short acc,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = (VarEnum)5)] ref Array hl,
            short trunc,
            ref double d_yield);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="maturity"></param>
        /// <param name="settleDate"></param>
        /// <param name="tradePrice"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="accrualMethod"></param>
        /// <returns></returns>
        public double aaZCB_y(
            decimal principal,
            DateTime? maturity,
            DateTime? settleDate,
            decimal tradePrice,
            Enumerations.CompoundingFrequency compoundingFrequency,
            Enumerations.AccrualMethod accrualMethod)
        {
            errorOccurred = false;
            errorMessage = string.Empty;

            double result = 0;
            short fincadResult = -1;
            double princ_m = General.ConvertDecimalToDouble(principal);
            double d_m = General.ConvertDateToDouble(maturity.Value);
            double d_v = General.ConvertDateToDouble(settleDate.Value);
            double price = General.ConvertDecimalToDouble(tradePrice);
            short cmpd_freq = (short)compoundingFrequency;
            short d_rul = (short)businessDayConvention;

            Array hl = holidayList;
            short acc = (short)accrualMethod;
            Array stat_147 = new double[1, 1];
            short trunc = 5;

            try
            {
                fincadResult = aaZCB_y(
                    princ_m,
                    d_m,
                    d_v,
                    price,
                    cmpd_freq,
                    d_rul,
                    acc,
                    ref hl,
                    trunc,
                    ref result);
            }
            catch (Exception exception)
            {
                errorOccurred = true;
                errorMessage = exception.ToString();
            }

            if (!errorOccurred && fincadResult == 0)
            {

            }
            else if (fincadResult == -1)
            {
                if (princ_m < 0)
                {
                    errorMessage = Messages.Zcb_1;
                }
                else if (d_m <= d_v)
                {
                    errorMessage = Messages.Zcb_2;
                }
                else if (price <= 0)
                {
                    errorMessage = Messages.Zcb_3;
                }
                else if (hl.Length <= 1)
                {
                    errorMessage = Messages.Zcb_4;
                }
                else
                {
                    errorMessage = Messages.GeneralError + " (aaZCB_y)";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="maturity"></param>
        /// <param name="cpn"></param>
        /// <param name="parCallDate"></param>
        /// <param name="premCallDate"></param>
        /// <param name="interestAccrDate"></param>
        /// <param name="firstCpnDate"></param>
        /// <param name="lastCpnDate"></param>
        /// <param name="tradePrice"></param>
        /// <param name="priceAtMaturity"></param>
        /// <param name="priceAtParCall"></param>
        /// <param name="priceAtPremiumCall"></param>
        /// <param name="accrualMethod"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="compoundingBasis"></param>
        /// <param name="exDividendDays"></param>
        /// <returns></returns>
        public double MuniYieldToWorst(
            DateTime settleDate,
            DateTime maturity,
            double coupon,
            DateTime? parCallDate,
            DateTime? premCallDate,
            DateTime? interestAccrDate,
            DateTime? firstCpnDate,
            DateTime? lastCpnDate,
            double tradePrice,
            double priceAtMaturity,
            double priceAtParCall,
            double priceAtPremiumCall,
            Enumerations.AccrualMethod accrualMethod,
            Enumerations.CompoundingFrequency compoundingFrequency,
            Enumerations.RateCompoundingBasis compoundingBasis,
            int exDividendDays)
        {
            double result = 0;
            double newPrice = 0;

            if (coupon == 0) //handle zero coupon bonds differently.
            {
                result = aaZCB_y(
                    (decimal)priceAtMaturity,
                    maturity,
                    settleDate,
                    (decimal)tradePrice,
                    compoundingFrequency,
                    accrualMethod) * 100;

                if (parCallDate != Convert.ToDateTime(null))
                {
                    newPrice = aaZCB_y(
                        (decimal)priceAtParCall,
                        parCallDate,
                        settleDate,
                        (decimal)tradePrice,
                        compoundingFrequency,
                        accrualMethod) * 100;

                    if (newPrice < result)
                    {
                        result = newPrice;
                    }
                }

                if (premCallDate != Convert.ToDateTime(null))
                {
                    newPrice = aaZCB_y(
                        (decimal)priceAtPremiumCall,
                        premCallDate,
                        settleDate,
                        (decimal)tradePrice,
                        compoundingFrequency,
                        accrualMethod) * 100;

                    if (newPrice < result)
                    {
                        result = newPrice;
                    }
                }
            }
            else
            {
                double[,] a = aaBond_y(
                    settleDate,
                    maturity,
                    interestAccrDate.Value,
                    firstCpnDate.Value,
                    GetLastCouponDate(maturity, (int)compoundingFrequency),
                    (decimal)coupon,
                    (decimal)priceAtMaturity,
                    compoundingFrequency,
                    accrualMethod,
                    (decimal)tradePrice,
                    compoundingBasis,
                    exDividendDays);

                result = a[0, 0] * 100;
                if (parCallDate != Convert.ToDateTime(null))
                {
                    if (priceAtParCall == 100)
                    {
                        a = aaBond_y(
                            settleDate,
                            parCallDate.Value,
                            interestAccrDate.Value,
                            firstCpnDate.Value,
                            GetLastCouponDateGivenDate(maturity, (int)compoundingFrequency, parCallDate),
                            (decimal)coupon,
                            (decimal)priceAtParCall,
                            compoundingFrequency,
                            accrualMethod,
                            (decimal)tradePrice,
                            compoundingBasis,
                            exDividendDays);

                        newPrice = a[0, 0] * 100;
                    }
                    else
                    {
                        newPrice = System.Numeric.Financial.Yield(
                            settleDate,
                            parCallDate.Value,
                            coupon / 100,
                            tradePrice,
                            priceAtParCall,
                            System.Numeric.Frequency.SemiAnnual,
                            System.Numeric.DayCountBasis.Europ30_360) * 100;
                    }

                    if (newPrice < result)
                    {
                        result = newPrice;
                    }
                }

                if (premCallDate != Convert.ToDateTime(null))
                {
                    if (priceAtPremiumCall == 100)
                    {
                        a = aaBond_y(
                            settleDate,
                            premCallDate.Value,
                            interestAccrDate.Value,
                            firstCpnDate.Value,
                            GetLastCouponDateGivenDate(maturity, (int)compoundingFrequency, parCallDate),
                            (decimal)coupon,
                            (decimal)priceAtPremiumCall,
                            compoundingFrequency,
                            accrualMethod,
                            (decimal)tradePrice,
                            compoundingBasis,
                            exDividendDays);

                        newPrice = a[0, 0] * 100;
                    }
                    else
                    {
                        newPrice = System.Numeric.Financial.Yield(
                            settleDate,
                            premCallDate.Value,
                            coupon / 100,
                            tradePrice,
                            priceAtPremiumCall,
                            System.Numeric.Frequency.SemiAnnual,
                            System.Numeric.DayCountBasis.Europ30_360) * 100;
                    }

                    if (newPrice < result)
                    {
                        result = newPrice;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="maturity"></param>
        /// <param name="coupon"></param>
        /// <param name="parCallDate"></param>
        /// <param name="premCallDate"></param>
        /// <param name="interestAccrDate"></param>
        /// <param name="firstCpnDate"></param>
        /// <param name="lastCpnDate"></param>
        /// <param name="tradePrice"></param>
        /// <param name="priceAtMaturity"></param>
        /// <param name="priceAtParCall"></param>
        /// <param name="priceAtPremiumCall"></param>
        /// <param name="accrualMethod"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="compoundingBasis"></param>
        /// <param name="exDividendDays"></param>
        /// <returns></returns>
        public DateTime WorstMaturityGivenPrice(
            DateTime settleDate,
            DateTime maturity,
            double coupon,
            DateTime parCallDate,
            DateTime premCallDate,
            DateTime interestAccrDate,
            DateTime firstCpnDate,
            DateTime lastCpnDate,
            double tradePrice,
            double priceAtMaturity,
            double priceAtParCall,
            double priceAtPremiumCall,
            Enumerations.AccrualMethod accrualMethod,
            Enumerations.CompoundingFrequency compoundingFrequency,
            Enumerations.RateCompoundingBasis compoundingBasis,
            int exDividendDays)
        {
            DateTime result;
            double worstPrice = 0;
            double newPrice = 0;

            if (coupon == 0) //handle zero coupon bonds differently.
            {
                worstPrice = aaZCB_y(
                    (decimal)priceAtMaturity,
                    maturity,
                    settleDate,
                    (decimal)tradePrice,
                    compoundingFrequency,
                    accrualMethod) * 100;

                result = maturity;
                if (parCallDate != Convert.ToDateTime(null))
                {
                    newPrice = aaZCB_y(
                        (decimal)priceAtParCall,
                        parCallDate,
                        settleDate,
                        (decimal)tradePrice,
                        compoundingFrequency,
                        accrualMethod) * 100;

                    if (newPrice < worstPrice)
                    {
                        worstPrice = newPrice;
                        result = parCallDate;
                    }
                }

                if (premCallDate != Convert.ToDateTime(null))
                {
                    newPrice = aaZCB_y(
                        (decimal)priceAtPremiumCall,
                        premCallDate,
                        settleDate,
                        (decimal)tradePrice,
                        compoundingFrequency,
                        accrualMethod) * 100;

                    if (newPrice < worstPrice)
                    {
                        worstPrice = newPrice;
                        result = premCallDate;
                    }
                }
            }
            else
            {
                double[,] a = aaBond_y(
                    settleDate,
                    maturity,
                    interestAccrDate,
                    firstCpnDate,
                    GetLastCouponDate(maturity, (int)compoundingFrequency),
                    (decimal)coupon,
                    (decimal)priceAtMaturity,
                    compoundingFrequency,
                    accrualMethod,
                    (decimal)tradePrice,
                    compoundingBasis,
                    exDividendDays);

                worstPrice = a[0, 0] * 100;
                result = maturity;
                if (parCallDate != Convert.ToDateTime(null))
                {
                    if (priceAtParCall == 100)
                    {
                        a = aaBond_y(
                            settleDate,
                            parCallDate,
                            interestAccrDate,
                            firstCpnDate,
                            GetLastCouponDateGivenDate(maturity, (int)compoundingFrequency, parCallDate),
                            (decimal)coupon,
                            (decimal)priceAtParCall,
                            compoundingFrequency,
                            accrualMethod,
                            (decimal)tradePrice,
                            compoundingBasis,
                            exDividendDays);

                        newPrice = a[0, 0] * 100;
                    }
                    else
                    {
                        newPrice = System.Numeric.Financial.Yield(
                            settleDate,
                            parCallDate,
                            coupon / 100,
                            tradePrice,
                            priceAtParCall,
                            System.Numeric.Frequency.SemiAnnual,
                            System.Numeric.DayCountBasis.Europ30_360) * 100;
                    }

                    if (newPrice < worstPrice)
                    {
                        worstPrice = newPrice;
                        result = parCallDate;
                    }
                }

                if (premCallDate != Convert.ToDateTime(null))
                {
                    if (priceAtPremiumCall == 100)
                    {
                        a = aaBond_y(
                            settleDate,
                            premCallDate,
                            interestAccrDate,
                            firstCpnDate,
                            GetLastCouponDateGivenDate(maturity, (int)compoundingFrequency, premCallDate),
                            (decimal)coupon,
                            (decimal)priceAtPremiumCall,
                            compoundingFrequency,
                            accrualMethod,
                            (decimal)tradePrice,
                            compoundingBasis,
                            exDividendDays);

                        newPrice = a[0, 0] * 100;
                    }
                    else
                    {
                        newPrice = System.Numeric.Financial.Yield(
                            settleDate,
                            premCallDate,
                            coupon / 100,
                            tradePrice,
                            priceAtPremiumCall,
                            System.Numeric.Frequency.SemiAnnual,
                            System.Numeric.DayCountBasis.Europ30_360) * 100;
                    }

                    if (newPrice < worstPrice)
                    {
                        worstPrice = newPrice;
                        result = premCallDate;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="maturity"></param>
        /// <param name="coupon"></param>
        /// <param name="parCallDate"></param>
        /// <param name="premCallDate"></param>
        /// <param name="interestAccrDate"></param>
        /// <param name="firstCpnDate"></param>
        /// <param name="lastCpnDate"></param>
        /// <param name="yield"></param>
        /// <param name="priceAtMaturity"></param>
        /// <param name="priceAtParCall"></param>
        /// <param name="priceAtPremiumCall"></param>
        /// <param name="accrualMethod"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="compoundingBasis"></param>
        /// <param name="exDividendDays"></param>
        /// <returns></returns>
        public double MuniPriceToWorst(
            DateTime settleDate,
            DateTime maturity,
            double coupon,
            DateTime parCallDate,
            DateTime premCallDate,
            DateTime interestAccrDate,
            DateTime firstCpnDate,
            DateTime lastCpnDate,
            double yield,
            double priceAtMaturity,
            double priceAtParCall,
            double priceAtPremiumCall,
            Enumerations.AccrualMethod accrualMethod,
            Enumerations.CompoundingFrequency compoundingFrequency,
            Enumerations.RateCompoundingBasis compoundingBasis,
            int exDividendDays)
        {
            double result = 0;
            DateTime worstMaturity = new DateTime();
            double newPrice = 0;
            double[,] a = null;

            if (coupon == 0) //handle zero coupon bonds differently.
            {
                a = aaZCB(
                    (decimal)priceAtMaturity,
                    maturity,
                    settleDate,
                    (decimal)yield / 100,
                    compoundingFrequency,
                    accrualMethod);

                result = a[0, 0];
                worstMaturity = maturity;

                if (parCallDate != Convert.ToDateTime(null) && parCallDate > settleDate)
                {
                    a = aaZCB(
                        (decimal)priceAtParCall,
                        parCallDate,
                        settleDate,
                        (decimal)yield / 100,
                        compoundingFrequency,
                        accrualMethod);

                    newPrice = a[0, 0];
                    if (newPrice < result)
                    {
                        result = newPrice;
                        worstMaturity = parCallDate;
                    }
                }

                if (premCallDate != Convert.ToDateTime(null) && premCallDate > settleDate)
                {
                    a = aaZCB(
                        (decimal)priceAtPremiumCall,
                        premCallDate,
                        settleDate,
                        (decimal)yield / 100,
                        compoundingFrequency,
                        accrualMethod);

                    newPrice = a[0, 0];
                    if (newPrice < result)
                    {
                        result = newPrice;
                        worstMaturity = premCallDate;
                    }
                }
            }
            else
            {
                lastCpnDate = GetLastCouponDate(maturity, (int)compoundingFrequency);
                a = aaBond_p(
                    settleDate,
                    maturity,
                    interestAccrDate,
                    firstCpnDate,
                    lastCpnDate,
                    (decimal)coupon,
                    (decimal)priceAtMaturity,
                    compoundingFrequency,
                    accrualMethod,
                    (decimal)yield / 100,
                    compoundingBasis,
                    exDividendDays);

                result = a[0, 0];
                worstMaturity = maturity;
                if (parCallDate != Convert.ToDateTime(null) && parCallDate > settleDate)
                {
                    if (priceAtParCall == 100)
                    {
                        lastCpnDate = GetLastCouponDateGivenDate(premCallDate, (int)compoundingFrequency, parCallDate);
                        a = aaBond_p(
                            settleDate,
                            parCallDate,
                            interestAccrDate,
                            firstCpnDate,
                            lastCpnDate,
                            (decimal)coupon,
                            (decimal)priceAtParCall,
                            compoundingFrequency,
                            accrualMethod,
                            (decimal)yield / 100,
                            compoundingBasis,
                            exDividendDays);

                        newPrice = a[0, 0];
                    }
                    else
                    {

                        newPrice = System.Numeric.Financial.Price(
                            settleDate,
                            parCallDate,
                            coupon / 100,
                            yield / 100,
                            priceAtParCall,
                            System.Numeric.Frequency.SemiAnnual,
                            System.Numeric.DayCountBasis.Europ30_360);
                    }

                    if (newPrice < result)
                    {
                        result = newPrice;
                        worstMaturity = parCallDate;
                    }
                }

                if (premCallDate != Convert.ToDateTime(null) && premCallDate > settleDate)
                {
                    if (priceAtPremiumCall == 100)
                    {
                        lastCpnDate = GetLastCouponDateGivenDate(premCallDate, (int)compoundingFrequency, premCallDate);
                        a = aaBond_p(
                            settleDate,
                            premCallDate,
                            interestAccrDate,
                            firstCpnDate,
                            lastCpnDate,
                            (decimal)coupon,
                            (decimal)priceAtPremiumCall,
                            compoundingFrequency,
                            accrualMethod,
                            (decimal)yield / 100,
                            compoundingBasis,
                            exDividendDays);

                        newPrice = a[0, 0];
                    }
                    else
                    {
                        newPrice = System.Numeric.Financial.Price(
                            settleDate,
                            premCallDate,
                            coupon / 100, yield / 100,
                            priceAtPremiumCall,
                            System.Numeric.Frequency.SemiAnnual,
                            System.Numeric.DayCountBasis.Europ30_360);
                    }

                    if (newPrice < result)
                    {
                        result = newPrice;
                        worstMaturity = premCallDate;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="maturity"></param>
        /// <param name="coupon"></param>
        /// <param name="parCallDate"></param>
        /// <param name="premCallDate"></param>
        /// <param name="interestAccrDate"></param>
        /// <param name="firstCpnDate"></param>
        /// <param name="lastCpnDate"></param>
        /// <param name="yield"></param>
        /// <param name="priceAtMaturity"></param>
        /// <param name="priceAtParCall"></param>
        /// <param name="priceAtPremiumCall"></param>
        /// <param name="accrualMethod"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="compoundingBasis"></param>
        /// <param name="exDividendDays"></param>
        /// <returns></returns>
        public DateTime WorstMaturityGivenYield(
            DateTime settleDate,
            DateTime maturity,
            double coupon,
            DateTime parCallDate,
            DateTime premCallDate,
            DateTime interestAccrDate,
            DateTime firstCpnDate,
            DateTime lastCpnDate,
            double yield,
            double priceAtMaturity,
            double priceAtParCall,
            double priceAtPremiumCall,
            Enumerations.AccrualMethod accrualMethod,
            Enumerations.CompoundingFrequency compoundingFrequency,
            Enumerations.RateCompoundingBasis compoundingBasis,
            int exDividendDays)
        {
            DateTime result = new DateTime();
            double worstPrice = 0;
            double newPrice = 0;
            double[,] a = null;

            if (coupon == 0) //handle zero coupon bonds differently.
            {
                a = aaZCB(
                    (decimal)priceAtMaturity,
                    maturity, settleDate,
                    (decimal)yield / 100,
                    compoundingFrequency,
                    accrualMethod);

                worstPrice = a[0, 0];
                result = maturity;
                if (parCallDate != Convert.ToDateTime(null))
                {
                    a = aaZCB(
                        (decimal)priceAtParCall,
                        parCallDate,
                        settleDate,
                        (decimal)yield / 100,
                        compoundingFrequency,
                        accrualMethod);

                    newPrice = a[0, 0];
                    if (newPrice < worstPrice)
                    {
                        worstPrice = newPrice;
                        result = parCallDate;
                    }
                }

                if (premCallDate != Convert.ToDateTime(null))
                {
                    a = aaZCB(
                        (decimal)priceAtPremiumCall,
                        premCallDate,
                        settleDate,
                        (decimal)yield / 100,
                        compoundingFrequency,
                        accrualMethod);

                    newPrice = a[0, 0];
                    if (newPrice < worstPrice)
                    {
                        worstPrice = newPrice;
                        result = premCallDate;

                    }
                }
            }
            else
            {
                a = aaBond_p(
                    settleDate,
                    maturity,
                    interestAccrDate,
                    firstCpnDate,
                    lastCpnDate,
                    (decimal)coupon,
                    (decimal)priceAtMaturity,
                    compoundingFrequency,
                    accrualMethod,
                    (decimal)yield / 100,
                    compoundingBasis,
                    exDividendDays);

                worstPrice = a[0, 0];
                result = maturity;
                if (parCallDate != Convert.ToDateTime(null))
                {
                    if (priceAtParCall == 100)
                    {
                        a = aaBond_p(
                            settleDate,
                            parCallDate,
                            interestAccrDate,
                            firstCpnDate,
                            new DateTime(),
                            (decimal)coupon,
                            (decimal)priceAtParCall,
                            compoundingFrequency,
                            accrualMethod,
                            (decimal)yield / 100,
                            compoundingBasis, exDividendDays);
                        newPrice = a[0, 0];
                    }
                    else
                    {
                        newPrice = System.Numeric.Financial.Price(
                            settleDate,
                            parCallDate,
                            coupon / 100,
                            yield / 100,
                            priceAtParCall,
                            System.Numeric.Frequency.SemiAnnual,
                            System.Numeric.DayCountBasis.Europ30_360);
                    }

                    if (newPrice < worstPrice)
                    {
                        worstPrice = newPrice;
                        result = parCallDate;

                    }
                }

                if (premCallDate != Convert.ToDateTime(null))
                {
                    if (priceAtPremiumCall == 100)
                    {
                        a = aaBond_p(settleDate, premCallDate, interestAccrDate, firstCpnDate, new DateTime(), (decimal)coupon, (decimal)priceAtPremiumCall, compoundingFrequency, accrualMethod, (decimal)yield / 100, compoundingBasis, exDividendDays);
                        newPrice = a[0, 0];
                    }
                    else
                    {
                        newPrice = System.Numeric.Financial.Price(
                            settleDate,
                            premCallDate,
                            coupon / 100,
                            yield / 100,
                            priceAtPremiumCall,
                            System.Numeric.Frequency.SemiAnnual,
                            System.Numeric.DayCountBasis.Europ30_360);
                    }

                    if (newPrice < worstPrice)
                    {
                        worstPrice = newPrice;
                        result = premCallDate;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settleDate"></param>
        /// <param name="maturity"></param>
        /// <param name="coupon"></param>
        /// <param name="parCallDate"></param>
        /// <param name="premCallDate"></param>
        /// <param name="interestAccrDate"></param>
        /// <param name="firstCpnDate"></param>
        /// <param name="lastCpnDate"></param>
        /// <param name="yield"></param>
        /// <param name="priceAtMaturity"></param>
        /// <param name="priceAtParCall"></param>
        /// <param name="priceAtPremiumCall"></param>
        /// <param name="accrualMethod"></param>
        /// <param name="compoundingFrequency"></param>
        /// <param name="compoundingBasis"></param>
        /// <param name="exDividendDays"></param>
        /// <returns></returns>
        public double MuniRisk(
            DateTime settleDate,
            DateTime maturity,
            double coupon,
            DateTime parCallDate,
            DateTime premCallDate,
            DateTime interestAccrDate,
            DateTime firstCpnDate,
            DateTime lastCpnDate,
            double yield,
            double priceAtMaturity,
            double priceAtParCall,
            double priceAtPremiumCall,
            Enumerations.AccrualMethod accrualMethod,
            Enumerations.CompoundingFrequency compoundingFrequency,
            Enumerations.RateCompoundingBasis compoundingBasis,
            int exDividendDays)
        {
            //Calculate risk at + 10 and -10 bps to current yield and then divide by 10.

            double px1 = MuniPriceToWorst(
                settleDate,
                maturity,
                coupon,
                parCallDate,
                premCallDate,
                interestAccrDate,
                firstCpnDate,
                lastCpnDate,
                yield + 0.10,
                priceAtMaturity,
                priceAtParCall,
                priceAtPremiumCall,
                accrualMethod,
                compoundingFrequency,
                compoundingBasis,
                exDividendDays);

            double px2 = MuniPriceToWorst(
                settleDate,
                maturity,
                coupon,
                parCallDate,
                premCallDate,
                interestAccrDate,
                firstCpnDate,
                lastCpnDate,
                yield - 0.10,
                priceAtMaturity,
                priceAtParCall,
                priceAtPremiumCall,
                accrualMethod,
                compoundingFrequency,
                compoundingBasis,
                exDividendDays);

            return (px1 - px2) / 20;
        }
    }
}
