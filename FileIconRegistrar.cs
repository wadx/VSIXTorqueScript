using System;
using Microsoft.VisualStudio.Shell;
using Microsoft.Win32;

namespace TorqueScriptLanguage
{
    /// <summary>
    /// Registers custom file icons via ShellFileAssociations in the running
    /// instance configuration hive. Workaround for pkgdef not being merged
    /// for MEF-only VSIXes. Runs on first opened .tscript/.gui file;
    /// a VS restart is required for the icon cache to refresh.
    /// </summary>
    internal static class FileIconRegistrar
    {
        // Must match the GUID in TorqueScriptImages.imagemanifest / TorqueScript.pkgdef
        private const string ImageGuid = "{9A5A9B8D-4709-4D0A-BB19-FAF82A16CAF4}";

        private static bool _done;

        public static void Register()
        {
            if (_done) return;
            _done = true;

            try
            {
                using (RegistryKey root = VSRegistry.RegistryRoot(Microsoft.VisualStudio.Shell.Interop.__VsLocalRegistryType.RegType_UserSettings))
                {
                    if (root == null) return;
                    SetIcon(root, ".tscript");
                    SetIcon(root, ".gui");
                }
            }
            catch (Exception)
            {
                // never break the editor because of icon registration
            }
        }

        private static void SetIcon(RegistryKey root, string extension)
        {
            using (RegistryKey key = root.CreateSubKey(@"ShellFileAssociations\" + extension))
            {
                key.SetValue("DefaultIconMoniker", ImageGuid + ":1");
            }
        }
    }
}
