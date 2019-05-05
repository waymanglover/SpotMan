using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace SpotMan.Helpers
{
    // TODO: Interface with separate implementations?
    public static class PersistenceHelper
    {
        private const string RegistryLocation = @"SOFTWARE\SpotMan";

        public static string GetKey(string name)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? GetKeyWindows(name) : GetKeyCrossPlatform(name);
        }

        private static string GetKeyWindows(string name)
        {
            return SpotManRegistryKey() == null ? string.Empty : SpotManRegistryKey().GetValue(name).ToString();
        }

        // ReSharper disable once UnusedParameter.Local
        private static string GetKeyCrossPlatform(string name)
        {
            throw new NotImplementedException();
        }

        public static Dictionary<string, string> GetKeys()
        {
            // TODO: Azure key vault?
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? GetKeysWindows() : GetKeysCrossPlatform();
        }

        private static Dictionary<string, string> GetKeysWindows()
        {
            var names = SpotManRegistryKey().GetValueNames();
            return names.ToDictionary(name => name, name => SpotManRegistryKey().GetValue(name).ToString());
        }

        private static RegistryKey SpotManRegistryKey()
        {
            return Registry.CurrentUser.OpenSubKey(RegistryLocation);
        }

        private static Dictionary<string, string> GetKeysCrossPlatform()
        {
            throw new NotImplementedException();
        }

        public static void StoreKey(string name, string value)
        {
            SpotManRegistryKey().SetValue(name, value);
        }

        public static void StoreKeys(Dictionary<string, string> keys)
        {
            // TODO: Azure key vault?
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                StoreKeysWindows(keys);
            else
                StoreKeysCrossPlatform(keys);
        }

        private static void StoreKeysWindows(Dictionary<string, string> keys)
        {
            foreach (var (name, value) in keys)
            {
                StoreKey(name, value);
            }
        }

        // ReSharper disable once UnusedParameter.Local
        private static void StoreKeysCrossPlatform(Dictionary<string, string> keys)
        {
            // TODO: JSON -> File for cross platform?
            throw new NotImplementedException();
        }
    }
}