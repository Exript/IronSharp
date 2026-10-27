using System.Collections.Generic;

namespace IronSharp.Utilities
{
    internal class CVECheck
    {
        public static void CheckAll()
        {
            var kblist = Utilities.GetWMIQuery.GetInstalledKBs();

            // GetBuildNumber zaten int döndürüyor, direkt alıyoruz.
            int buildNumber = Utilities.GetWMIQuery.GetBuildNumber();

            // Yeni dinamik motoru çağırıyoruz
            DynamicCveEngine.CheckAllDynamic(buildNumber, kblist);
        }
    }
}