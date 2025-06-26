using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using project.Models;

namespace project.Services
{
    public static class CheckService
    {
        private static readonly string FilePath = "checks.json";
        private static readonly object _lock = new object();
        private static List<Check> _checks = null;

        public static void AddCheck(Check check)
        {
            lock (_lock)
            {
                LoadChecks();
                _checks.Add(check);
                SaveChecks();
            }
        }

        public static List<Check> GetAllChecks()
        {
            lock (_lock)
            {
                LoadChecks();
                return new List<Check>(_checks);
            }
        }

        private static void LoadChecks()
        {
            if (_checks != null) return;
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                _checks = JsonSerializer.Deserialize<List<Check>>(json) ?? new List<Check>();
            }
            else
            {
                _checks = new List<Check>();
            }
        }

        private static void SaveChecks()
        {
            var json = JsonSerializer.Serialize(_checks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
} 