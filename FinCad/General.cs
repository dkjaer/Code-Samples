using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace FinCad
{
    /// <summary>
    /// 
    /// </summary>
    public class General
    {
        DataTable CreateDataTable(PropertyInfo[] Properties)
        {
            DataTable dt = new DataTable();

            foreach (PropertyInfo pi in Properties)
            {
                DataColumn dc = new DataColumn();
                dc.ColumnName = pi.Name;
                dc.DataType = pi.PropertyType;
                dt.Columns.Add(dc);
            }

            return dt;
        }

        /// <summary>
        /// Returns the name of the method that called this exception.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetExecutingMethodName(Type type)
        {
            string result = "Unknown source method.";
            StackTrace trace = new StackTrace(false);
            for (int index = 0; index < trace.FrameCount - 2; ++index)
            {
                StackFrame frame = trace.GetFrame(index);
                MethodBase method = frame.GetMethod();
                if (method.DeclaringType != type && !type.IsAssignableFrom(method.DeclaringType))
                {
                    frame = trace.GetFrame(index + 2);
                    method = frame.GetMethod();
                    result = string.Concat(method.DeclaringType.FullName, ".", method.Name);
                    break;
                }
            }

            return result.Replace("ASA.BusinessLogic.ProfitAndLoss.", string.Empty);
        }

        public static double ConvertDateToDouble(DateTime Date)
        {
            double result = 0;
            try
            {
                result = Date.ToOADate();
            }
            catch { }
            return result;
        }

        public static DateTime ConvertDoubleToDate(double Double)
        {
            DateTime result = Convert.ToDateTime(null);
            try
            {
                result = DateTime.FromOADate(Double);
            }
            catch { }
            return result;
        }

        public static double ConvertDecimalToDouble(decimal Price)
        {
            double result = 0;
            try { result = Convert.ToDouble(Price); }
            catch { }
            return result;
        }

        public static string ConvertPeriodToDate(string strPeriod, string strFormat)
        {
            string result = string.Empty;

            try
            {
                int intMonthIn = Convert.ToInt32(strPeriod.Substring(4, 2));
                int intYearIn = Convert.ToInt32(strPeriod.Substring(0, 4));
                DateTime dtIn = new DateTime(intYearIn, intMonthIn, 1);
                result = dtIn.ToString(strFormat);
            }
            catch (Exception exception)
            {
                string strEx = exception.ToString();
            }

            return result;
        }

        public static decimal ConvertStringToDecimal(string strAmount)
        {
            decimal result = 0;
            int intMaxdecimals = 10;

            strAmount = strAmount.Replace("$", "");
            strAmount = strAmount.Replace(",", "");

            try
            {
                //result = Convert.ToDecimal( strAmount );


                try
                {
                    string strTemp = strAmount;
                    if (strTemp.IndexOf(".") > 0)
                    {
                        string[] strSplit = strTemp.Split('.');
                        string strDecimals = strSplit[1];
                        if (strDecimals.Length > intMaxdecimals)
                        {
                            strDecimals = strDecimals.Substring(0, intMaxdecimals);
                            strTemp = strSplit[0] + "." + strDecimals;
                        }
                    }

                    decimal decTemp = Convert.ToDecimal(strTemp);

                    result = decTemp;
                }
                catch { }


            }
            catch
            { }
            return result;
        }

        public static double ConvertStringToDouble(string strAmount)
        {
            double result = 0;
            strAmount = strAmount.Replace("$", "");
            strAmount = strAmount.Replace(",", "");
            try
            {
                result = Convert.ToDouble(strAmount);
            }
            catch
            { }
            return result;
        }

        public static int ConvertStringToInteger(string strAmount)
        {
            int result = 0;
            strAmount = strAmount.Replace("$", "");
            strAmount = strAmount.Replace(",", "");
            try
            {
                result = Convert.ToInt32(strAmount);
            }
            catch
            { }
            return result;
        }

        public static DateTime ConvertStringToDate(string InString)
        {
            DateTime result = Convert.ToDateTime(null);
            try
            {
                result = Convert.ToDateTime(InString);
            }
            catch { }
            return result;
        }

        public static DataSet ConvertArraylistToDataset(string table_name, ArrayList arraylist)
        {
            DataSet m_ds = new DataSet();

            m_ds.Tables.Add(table_name);
            DataTable m_dt = m_ds.Tables[0];

            for (int i = 0; i < arraylist.Count; i++)
            {
                m_dt.Columns.Add();
            }

            IEnumerator m_enumerator = arraylist.GetEnumerator();
            while (m_enumerator.MoveNext())
            {
                DataRow m_dr = m_dt.NewRow();

                for (int i = 0; i < arraylist.Count; i++)
                {
                    m_dr[i] = arraylist[i];
                }

                m_dt.Rows.Add(m_dr);
            }

            return m_ds;
        }

        public static List<T> ConvertArraylistToList<T>(ArrayList arrayList)
        {
            List<T> list = new List<T>(arrayList.Count);
            foreach (T instance in arrayList)
            {
                list.Add(instance);
            }
            return list;
        }

        public static int ConvertFrequencyToMonths(int Frequency)
        {
            int result = 0;

            try { result = 12 / Frequency; }
            catch { }

            /*
            switch( Frequency )
            {
                case 1: result = 12; break;	// annual
                case 2: result = 6; break;	// semi-annual
                case 4: result = 3; break;	// quarterly
            }
            */
            return result;
        }

        public static string ConvertFrequencyToString(int Months)
        {
            string result = string.Empty;

            switch (Months)
            {
                case 12: result = "Annual"; break;
                case 6: result = "Semi-Annual"; break;
                case 3: result = "Quarterly"; break;
                case 1: result = "Monthly"; break;
            }

            return result;
        }

        public static decimal ConvertFractionStringToDecimal(string strFraction)
        {
            decimal result = 0;

            int intSlashLocation = 0;
            string strLeft = string.Empty;
            string strRight = string.Empty;
            decimal decLeft = 0;
            decimal decRight = 0;

            strFraction = strFraction.Trim();
            intSlashLocation = strFraction.IndexOf("/", 1);
            if (intSlashLocation > 0)
            {
                strLeft = strFraction.Substring(0, intSlashLocation);
                strRight = strFraction.Substring(strFraction.Length - intSlashLocation);

                try { decLeft = Convert.ToDecimal(strLeft); }
                catch { }

                try { decRight = Convert.ToDecimal(strRight); }
                catch { }

                if (decRight > 0)
                    result = decLeft / decRight;
            }

            return result;
        }

        public static int ConvertDOWToInt(DayOfWeek DoW)
        {
            int result = -1;

            switch (DoW)
            {
                case DayOfWeek.Sunday: result = 0; break;
                case DayOfWeek.Monday: result = 1; break;
                case DayOfWeek.Tuesday: result = 2; break;
                case DayOfWeek.Wednesday: result = 3; break;
                case DayOfWeek.Thursday: result = 4; break;
                case DayOfWeek.Friday: result = 5; break;
                case DayOfWeek.Saturday: result = 6; break;
            }

            return result;
        }

        //public static DataTable ConvertObjectToDataTable(object o)
        //{
        //	PropertyInfo[] Properties = o.GetType().GetProperties();
        //	DataTable dt = CreateDataTable(Properties);
        //	FillData(Properties, dt, o);
        //	return dt;
        //}

        public static int ConvertMonthCodeToInt(string MonthCode)
        {
            int _iResult = 0;

            switch (MonthCode)
            {
                case "F": _iResult = 1; break;
                case "G": _iResult = 2; break;
                case "H": _iResult = 3; break;
                case "J": _iResult = 4; break;
                case "K": _iResult = 5; break;
                case "M": _iResult = 6; break;
                case "N": _iResult = 7; break;
                case "Q": _iResult = 8; break;
                case "U": _iResult = 9; break;
                case "V": _iResult = 10; break;
                case "X": _iResult = 11; break;
                case "Z": _iResult = 12; break;
            }

            return _iResult;
        }
    }
}