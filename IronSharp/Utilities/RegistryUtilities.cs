using Microsoft.Win32;
using System;
using System.Management;
using System.Security.AccessControl;
using System.Security.Principal;

namespace IronSharp.Utilities
{
    internal class RegistryUtilities
    {
        public static string GetRegistryValue(string hive, string path, string value)
        {
            string registryKeyValue = null;
            if (hive == "HKCU")
            {
                var regKey = Registry.CurrentUser.OpenSubKey(path);
                if (regKey != null)
                {
                    registryKeyValue = regKey.GetValue(value)?.ToString();
                }
                return registryKeyValue;
            }
            else if (hive == "HKU")
            {
                var regKey = Registry.Users.OpenSubKey(path);
                if (regKey != null)
                {
                    registryKeyValue = regKey.GetValue(value)?.ToString();
                }
                return registryKeyValue;
            }
            else
            {
                var regKey = Registry.LocalMachine.OpenSubKey(path);
                if (regKey != null)
                {
                    registryKeyValue = regKey.GetValue(value)?.ToString();
                }
                return registryKeyValue;
            }
        }

        public static string GetReleaseIdFromRegistiry()
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                var displayVersion = key.GetValue("ReleaseId")?.ToString();
                return displayVersion;
            }
            catch (ManagementException e)
            {
                Console.WriteLine("[!] {0}", e.Message);
                throw;
            }
            catch (NullReferenceException)
            {
                return "Unknown";
            }
        }

        public static string GetVersionFromRegistry()
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                var displayVersion = key.GetValue("DisplayVersion")?.ToString();
                return displayVersion;
            }
            catch (ManagementException e)
            {
                Console.WriteLine("[!] {0}", e.Message);
                throw;
            }
            catch (NullReferenceException)
            {
                return "";
            }
        }

        // DÜZELTİLEN KISIM BURASI:
        public static bool IsModifiable(RegistryKey key)
        {
            try
            {
                var security = key.GetAccessControl();
                var rules = security.GetAccessRules(true, true, typeof(SecurityIdentifier));

                foreach (RegistryAccessRule rule in rules)
                {
                    if (rule.AccessControlType == AccessControlType.Allow)
                    {
                        string sid = rule.IdentityReference.Value;

                        // Sadece Everyone (S-1-1-0), Users (S-1-5-32-545) ve Authenticated Users (S-1-5-11) gruplarına bakıyoruz.
                        // Yönetici yetkisi olsa bile bu gruplar üzerinden kontrol yapıyoruz.
                        if (sid == "S-1-1-0" || sid == "S-1-5-32-545" || sid == "S-1-5-11")
                        {
                            if ((rule.RegistryRights & RegistryRights.SetValue) == RegistryRights.SetValue ||
                                (rule.RegistryRights & RegistryRights.FullControl) == RegistryRights.FullControl ||
                                (rule.RegistryRights & RegistryRights.WriteKey) == RegistryRights.WriteKey)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch { }
            return false;
        }
    }
}