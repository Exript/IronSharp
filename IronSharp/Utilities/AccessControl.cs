using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace IronSharp.Utilities
{
    class AccessControl
    {
        public static void CheckPermission(string dir, FileSystemRights AccessRight)
        {
            try
            {
                // Yönetici olsak bile, "Everyone" veya "Users" grubunun yazma yetkisi olup olmadığına bakıyoruz.
                AuthorizationRuleCollection rules = new DirectoryInfo(dir).GetAccessControl().GetAccessRules(true, true, typeof(SecurityIdentifier));

                foreach (FileSystemAccessRule rule in rules)
                {
                    // S-1-1-0 = Everyone, S-1-5-32-545 = Users
                    string sid = rule.IdentityReference.Value;
                    if (sid == "S-1-1-0" || sid == "S-1-5-32-545")
                    {
                        if ((AccessRight & rule.FileSystemRights) == AccessRight)
                        {
                            if (rule.AccessControlType == AccessControlType.Allow)
                            {
                                Console.WriteLine("[+] {0} : Writable", dir);
                                return; // Bir kez bulması yeterli, döngüden çık.
                            }
                        }
                    }
                }
            }
            catch { }
        }
    }
}