using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace SpotMan.Helpers
{
    public static class PersistenceHelper
    {
        private const string RegistryLocation = @"SOFTWARE\SpotMan";

        public static void StoreKeys(Dictionary<string, string> keys)
        {
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
