using UnityEditor;
using UnityEditor.SceneTemplate;
using UnityEngine;


namespace VirtualSkateMappingTools
{
    public class VSMTCreateTools
    {
        public delegate void MapCreationCompletedHandler(bool mapCreationSuccessful);
        public static event MapCreationCompletedHandler MapCreationCompleted;
        private static bool _isBuilding = false;

        public static void CreateNewMap(string mapName)
        {
            var mapCreationResult = false;
            try
            {

                if (_isBuilding)
                {
                    Debug.LogWarning("Map creation already in progress!");
                    return;
                }

                if (string.IsNullOrEmpty(mapName))
                {
                    Debug.LogError("Please provide a valid map name");
                    return;
                }

                Debug.Log("Attempting to create a new map...");
                _isBuilding = true;

                var mapNameCleaned = mapName.Replace(" ", "_");
                var assetBundleName = mapNameCleaned.ToLower();

                var template = AssetDatabase.LoadAssetAtPath<SceneTemplateAsset>("Assets/Editor/VirtualSkateMappingTools/VSMT.scenetemplate");
                string newScenePath = $"Assets/Maps/{mapNameCleaned}/{mapNameCleaned}.unity";

                var result = SceneTemplateService.Instantiate(template, false, newScenePath);
                if (result != null)
                {
                    AssetImporter sceneImporter = AssetImporter.GetAtPath(newScenePath);
                    if (sceneImporter != null)
                    {
                        sceneImporter.assetBundleName = assetBundleName;
                        sceneImporter.SaveAndReimport();
                        Debug.Log($"Map created successful: {newScenePath} (AssetBundle: {sceneImporter.assetBundleName})");
                    }
                    else
                    {
                        Debug.LogError($"Failed to get AssetImporter for new map at: {newScenePath}");
                    }
                }
                else
                {
                    Debug.LogError("Failed to create map from template");
                }
            }
            finally
            {
                _isBuilding = false;
                MapCreationCompleted?.Invoke(mapCreationResult);
            }
        }
    }
}
