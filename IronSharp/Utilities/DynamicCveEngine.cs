using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace IronSharp.Utilities
{
    public class CveData
    {
        public string CveId { get; set; }
        public string Description { get; set; }
        public Dictionary<string, List<int>> AffectedBuilds { get; set; }
    }

    internal class DynamicCveEngine
    {
        public static void CheckAllDynamic(int currentBuildNumber, List<int> installedKblist)
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cves.json");

            if (!File.Exists(jsonPath))
            {
                Console.WriteLine("    [-] cves.json bulunamadi.");
                return;
            }

            try
            {
                string jsonString = File.ReadAllText(jsonPath);
                var cveList = JsonSerializer.Deserialize<List<CveData>>(jsonString);

                foreach (var cve in cveList)
                {
                    // Mevcut build numarası JSON'da tanımlı mı?
                    string buildStr = currentBuildNumber.ToString();
                    if (cve.AffectedBuilds.ContainsKey(buildStr))
                    {
                        var patchKbs = cve.AffectedBuilds[buildStr];

                        // Eğer yamayı kapatan KB kurulu DEĞİLSE (!Intersect)
                        if (patchKbs.Count > 0 && !patchKbs.Intersect(installedKblist).Any())
                        {
                            Console.WriteLine($"    [!] Vulnerable for {cve.CveId} - {cve.Description}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("    [-] JSON okuma hatasi: " + ex.Message);
            }
        }
    }
}