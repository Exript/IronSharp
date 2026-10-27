using System;
using System.Management;

namespace IronSharp.Utilities
{
    public class GetAv
    {
        public static void CheckSecurityProduct()
        {
            try
            {
                // SecurityCenter2 üzerinden yüklü antivirüsleri sorguluyoruz
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\SecurityCenter2", "SELECT * FROM AntiVirusProduct"))
                {
                    bool found = false;
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string name = obj["displayName"]?.ToString();
                        Console.WriteLine($"    [+] Installed AV: {name}");
                        found = true;
                    }

                    if (!found)
                    {
                        Console.WriteLine("    [-] No Antivirus product found via WMI.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("    [-] AV sorgusu basarisiz: " + ex.Message);
            }
        }
    }
}