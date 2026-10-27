using System;
using System.Collections.Generic;
using System.Linq;

namespace IronSharp.CVE
{
    internal class CVE_2024_26241
    {
        public static void Check(int buildnumber, List<int> installedkblist)
        {
            var vulnkblist = new List<int>();
            switch (buildnumber)
            {
                case 22621: // Windows 11 22H2
                    vulnkblist.AddRange(new int[] { 5037771 });
                    break;
                case 22631: // Windows 11 23H2
                    vulnkblist.AddRange(new int[] { 5037771 });
                    break;
                case 26100: // Windows 11 24H2
                    vulnkblist.AddRange(new int[] { 5037770 });
                    break;
                case 26200: // Sizin kullandığınız Windows 11 25H2
                    vulnkblist.AddRange(new int[] { 5037772 }); // Örnek bir KB numarası
                    break;
                default:
                    break;
            }

            // DÜZELTİLEN MANTIK: Yama (KB) listede YOKSA ( ! ) zafiyet vardır.
            if (vulnkblist.Count > 0 && !vulnkblist.Intersect(installedkblist).Any())
            {
                Console.WriteLine("[!] Vulnerable for CVE-2024-26241 (LPE)");
            }
        }
    }
}