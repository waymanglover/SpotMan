using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace SpotMan.Helpers
{
    // TODO: Interface with separate implementations?
    public static class PersistenceHelper
    {
        private const string RegistryLocation = @"SOFTWARE\SpotMan";
        public static string GetKey()
        {
            var subKey = Registry.CurrentUser.OpenSubKey(RegistryLocation);
            if (subKey == null) return string.Empty;
            return string.Empty;
        }
        public static Dictionary<string, string> GetKeys()
        {
            // TODO: Azure key vault?
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? GetKeysWindows() : GetKeysJson();
        }

        private static Dictionary<string, string> GetKeysJson()
        {
            throw new System.NotImplementedException();
        }

        private static Dictionary<string, string> GetKeysWindows()
        {
            var subKey = Registry.CurrentUser.OpenSubKey(RegistryLocation);
            var output = new Dictionary<string, string>();
            if (subKey == null) return output;

            var names = subKey.GetValueNames();
            foreach (var name in names)
            {
                output.Add(name, subKey.GetValue(name).ToString());
            }
            return output;
        }

        public static void StoreKeys(Dictionary<string, string> keys)
        {
            // TODO: Azure key vault?
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                StoreKeysWindows(keys);
            }
            else
            {
                StoreKeysJson(keys);
            }
        }

        private static void StoreKeysJson(Dictionary<string, string> keys)
        {
            // TODO: JSON -> File for cross platform?
            throw new System.NotImplementedException();
        }

        private static void StoreKeysWindows(Dictionary<string, string> keys)
        {
            using var registryKey = Registry.CurrentUser.CreateSubKey(RegistryLocation);
            foreach (var (name, value) in keys)
            {
                Debug.Assert(registryKey != null, nameof(registryKey) + " != null");
                registryKey.SetValue(name, value);
            }
        }
    }
}
