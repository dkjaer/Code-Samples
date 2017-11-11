using System;
using System.Diagnostics;

namespace Sharepoint
{
    public static class Registry
    {
        public const string SharepointUserKey = "SharepointUser";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetKey(string key)
        {
            string result = string.Empty;

            try
            {
                Microsoft.Win32.RegistryKey registryKey =
                    Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("Alternative Strategy Advisers");
                result = (string)registryKey.GetValue(key);
                registryKey.Close();
                if (result == null)
                {
                    result = string.Empty;
                }
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetKey(string key, string value)
        {
            var result = false;
            try
            {
                var registryKey = Microsoft.Win32.Registry.CurrentUser
                    .CreateSubKey("Software")
                    .CreateSubKey("Alternative Strategy Advisers");
                registryKey.SetValue(key, value);
                registryKey.Close();
                result = true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }

            return result;
        }
    }
}
