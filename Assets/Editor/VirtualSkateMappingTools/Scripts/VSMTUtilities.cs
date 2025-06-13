
using System;
using System.IO;
using System.Linq;

using UnityEditor;
using UnityEngine;


namespace VirtualSkateMappingTools
{
    public static class VSMTUtilities
    {
        public static event Action OnAssetBundlesChanged;
        private static string[] _lastAssetBundleNames = new string[] { };

        static VSMTUtilities()
        {
            EditorApplication.update += CheckForChanges;
        }

        public static void CheckForChanges()
        {
            var currentBundles = AssetDatabase.GetAllAssetBundleNames();
            if (!currentBundles.SequenceEqual(_lastAssetBundleNames))
            {
                _lastAssetBundleNames = currentBundles;
                OnAssetBundlesChanged?.Invoke();
            }
        }

        public static void CopyAssetBundleToGameDir(string assetBundleName)
        {
            string assetBundlePath = Path.Combine(Application.dataPath, "../AssetBundles", assetBundleName);
            string gameDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Virtual Skate",
                "Mod Maps"
            );
            string destPath = Path.Combine(gameDir, assetBundleName);

            try
            {
                File.Copy(assetBundlePath, destPath, overwrite: true);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to copy \"{assetBundleName}\" into the game directory!");
                Debug.LogError(e.Message);
                return;
            }

            Debug.Log($"Successfully copied \"{assetBundleName}\" into the game directory: {destPath}");
        }
    }
}
