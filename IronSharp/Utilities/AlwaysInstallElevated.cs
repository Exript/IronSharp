using Microsoft.Win32;
using System;

namespace IronSharp.Utilities
{
    internal class AlwaysInstallElevated
    {
        public static void AlwaysInstallElevatedCheck()
        {
            string subKey = @"SOFTWARE\Policies\Microsoft\Windows\Installer";

            // Hem LocalMachine hem de CurrentUser kontrol edilmeli
            string hklmVal = ReadFromRegistry("AlwaysInstallElevated", RegistryHive.LocalMachine, subKey);
            string hkcuVal = ReadFromRegistry("AlwaysInstallElevated", RegistryHive.CurrentUser, subKey);

            if (hklmVal == "1" && hkcuVal == "1")
            {
                Console.WriteLine("[!] Always Install Elevated Is Enabled (Vulnerable).");
            }
        }

        static string ReadFromRegistry(string keyName, RegistryHive hive, string subKey)
        {
            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default))
                using (RegistryKey sk = baseKey.OpenSubKey(subKey))
                {
                    if (sk == null) return null;
                    object val = sk.GetValue(keyName.ToUpper());
                    return val?.ToString(); // Null ise çökmez, null döner
                }
            }
            catch { return null; }
        }
    }
}