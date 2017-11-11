namespace FinCad
{
    /// <summary>
    /// 
    /// </summary>
    public class Messages
    {
        /// <summary>
        /// FinCad validation messages
        /// </summary>
        public const string GeneralError = "FinCAD error : Unable to determine cause.";

        public const string BL_iv_1 = "Underlying price must be greater than 0.";
        public const string BL_iv_2 = "Exercise price must be greater than 0.";
        public const string BL_iv_3 = "Expiration date must be after the value date.";
        public const string BL_iv_4 = "Annual rate must be equal or greater than 0.";

        public const string Bond_CA_y_1 = "Maturity date must be after the settle date.";
        public const string Bond_CA_y_2 = "Dated date must be on or before the settle date.";
        public const string Bond_CA_y_3 = "First coupon date must be after the dated date.";
        public const string Bond_CA_y_4 = "The coupon must be 0 or greater.";
        public const string Bond_CA_y_5 = "The principal must be greater than 0.";
        public const string Bond_CA_y_6 = "The price must be greater than 0.";

        public const string Bond_US_y_1 = "Maturity date must be after the settle date.";
        public const string Bond_US_y_2 = "Dated date must be on or before the settle date.";
        public const string Bond_US_y_3 = "First coupon date must be after the dated date.";
        public const string Bond_US_y_4 = "The coupon must be 0 or greater.";
        public const string Bond_US_y_5 = "The principal must be greater than 0.";
        public const string Bond_US_y_6 = "The price must be greater than 0.";

        public const string Bond_cf_1 = "Maturity date must be after the settle date.";
        public const string Bond_cf_2 = "Dated date must be on or before the settle date.";
        public const string Bond_cf_3 = "First coupon date must be after the dated date.";
        public const string Bond_cf_4 = "Last coupon date must be prior to maturity date.";
        public const string Bond_cf_5 = "The coupon must be 0 or greater.";
        public const string Bond_cf_6 = "The principal must be greater than 0.";
        public const string Bond_cf_7 = "No holiday list data provided.";

        public const string Bond_p_1 = "Maturity date must be after the settle date.";
        public const string Bond_p_2 = "Dated date must be on or before the settle date.";
        public const string Bond_p_3 = "First coupon date must be after the dated date.";
        public const string Bond_p_4 = "Last coupon date must be prior to maturity date.";
        public const string Bond_p_5 = "The coupon must be 0 or greater.";
        public const string Bond_p_6 = "The principal must be greater than 0.";
        public const string Bond_p_7 = "The yield must be greater than 0.";

        public const string Bond_y_1 = "Maturity date must be after the settle date.";
        public const string Bond_y_2 = "Dated date must be before the settle date.";
        public const string Bond_y_3 = "First coupon date must be after the dated date.";
        public const string Bond_y_4 = "Last coupon date must be prior to maturity date.";
        public const string Bond_y_5 = "The coupon must be 0 or greater.";
        public const string Bond_y_6 = "The principal must be greater than 0.";
        public const string Bond_y_7 = "The price must be greater than 0.";

        public const string Fixlg_accrued_1 = "Terminating date must be after the effective date.";
        public const string Fixlg_accrued_2 = "First coupon date must be after the dated date.";
        public const string Fixlg_accrued_3 = "Last coupon date must be prior to maturity date.";
        public const string Fixlg_accrued_4 = "The coupon must be 0 or greater.";
        public const string Fixlg_accrued_5 = "The principal must be greater than 0.";
        public const string Fixlg_accrued_6 = "No holiday list data provided.";

        public const string Fixlg_cfx2_1 = "Settle date must be the same as the first date in RateCurve.";
        public const string Fixlg_cfx2_2 = "Settle date must be on or before the last cash flow date.";
        public const string Fixlg_cfx2_3 = "Maturity must be after the settle date.";
        public const string Fixlg_cfx2_4 = "Maturity must be on or before the last cash flow date.";
        public const string Fixlg_cfx2_5 = "No snapshot data provided.";
        public const string Fixlg_cfx2_6 = "No holiday list data provided.";

        public const string Fixlg_p_1 = "Terminating date must be after the effective date.";
        public const string Fixlg_p_2 = "First coupon date must be after the dated date.";
        public const string Fixlg_p_3 = "Last coupon date must be prior to maturity date.";
        public const string Fixlg_p_4 = "The coupon must be 0 or greater.";
        public const string Fixlg_p_5 = "The principal must be greater than 0.";
        public const string Fixlg_p_6 = "No snapshot data provided.";
        public const string Fixlg_p_7 = "No holiday list data provided.";

        public const string Interp_1 = "No date list provided.";
        public const string Interp_2 = "No snapshot data provided.";

        public const string ParSwap_1 = "Terminating date must be after the effective date.";
        public const string ParSwap_2 = "First coupon date must be after the dated date.";
        public const string ParSwap_3 = "Last coupon date must be prior to maturity date.";
        public const string ParSwap_4 = "No snapshot data provided.";
        public const string ParSwap_5 = "No holiday list data provided.";

        public const string Zcb_1 = "The principal must be greater than 0.";
        public const string Zcb_2 = "Maturity date must be after the settle date.";
        public const string Zcb_3 = "The yield must be greater than 0.";
        public const string Zcb_4 = "No holiday list data provided.";
    }
}
