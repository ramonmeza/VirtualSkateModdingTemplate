using System.IO;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


namespace VirtualSkateMappingTools
{
    public class VSMTBuildTools
    {
        public delegate void BuildCompletedHandler(bool buildSuccessful);
        public static event BuildCompletedHandler BuildCompleted;
        private static bool _isBuilding = false;

        public static void BuildCurrentMap()
        {
            var buildResult = false;
            try
            {
                if (_isBuilding)
                {
                    Debug.LogWarning("Build already in progress!");
                    return;
                }

                Debug.Log("Attempting to build current map...");
                _isBuilding = true;

                var currentMap = EditorSceneManager.GetActiveScene();
                if (!currentMap.isLoaded)
                {
                    Debug.LogError("No scene is currently open or loaded!");
                    return;
                }

                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    Debug.LogWarning("You must save the current map to avoid unexpected results.");
                    return;
                }

                var mapAssetBundleName = currentMap.name.Replace(" ", "_").ToLower();
                var mapAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(currentMap.path);
                AssetImporter.GetAtPath(currentMap.path).SetAssetBundleNameAndVariant(mapAssetBundleName, "");
                AssetDatabase.SaveAssets();
                Debug.Log($"Map assigned to AssetBundle \"{mapAssetBundleName}\"");

                string outputPath = Path.Combine(Application.dataPath, "..", "AssetBundles");
                if (!Directory.Exists(outputPath))
                {
                    Debug.Log("AssetBundle directory created.");
                    Directory.CreateDirectory(outputPath);
                }



                Debug.Log("Building AssetBundle...");
                AssetBundleBuild buildMap = new AssetBundleBuild
                {
                    assetBundleName = mapAssetBundleName,
                    assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(mapAssetBundleName)
                };
                var target = EditorUserBuildSettings.activeBuildTarget;
                BuildPipeline.BuildAssetBundles(outputPath, new[] { buildMap }, BuildAssetBundleOptions.None, target);
                EditorApplication.delayCall += AssetDatabase.Refresh;
                Debug.Log($"Map build successful: {outputPath}");
            }
            finally
            {
                _isBuilding = false;
                BuildCompleted?.Invoke(buildResult);
            }
        }
    }
}
