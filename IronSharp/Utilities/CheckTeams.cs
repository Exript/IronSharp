using System;
using System.IO;
using System.Security.AccessControl;

namespace IronSharp.Utilities
{
    public class CheckTeams
    {
        public static void TeamsInstalled()
        {
            // Klasik Teams dizini
            string teamsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Teams");

            if (Directory.Exists(teamsPath))
            {
                Console.WriteLine($"    [+] Microsoft Teams found at: {teamsPath}");

                // Klasöre yazma izni olup olmadığını kontrol et (Yetki Yükseltme / Side-Loading riski)
                Utilities.AccessControl.CheckPermission(teamsPath, FileSystemRights.Write);
            }
            else
            {
                // Yeni Teams (Windows 11 içerisinde gömülü gelen) dizini
                string newTeamsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\WindowsApps");
                if (File.Exists(Path.Combine(newTeamsPath, "ms-teams.exe")))
                {
                    Console.WriteLine("    [+] New Microsoft Teams (Windows 11) is installed.");
                }
                else
                {
                    Console.WriteLine("    [-] Microsoft Teams is not installed.");
                }
            }
        }
    }
}