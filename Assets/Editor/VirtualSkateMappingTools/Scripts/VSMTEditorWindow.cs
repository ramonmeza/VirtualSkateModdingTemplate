using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


namespace VirtualSkateMappingTools
{
    public class VSMTEditorWindow : EditorWindow
    {
        private ScrollView _assetBundleScrollView;


        public static void OpenWindow()
        {
            var window = GetWindow<VSMTEditorWindow>();
            window.titleContent = new GUIContent("Virtual Skate Mapping Tools");
        }

        public void CreateGUI()
        {
            // load stylesheet
            var assetPath = Path.Combine(new string[] {
                "Assets",
                "Editor",
                "VirtualSkateMappingTools",
                "Scripts",
                "VirtualSkateMappingToolsStyles.uss"
            }).Replace("\\", "/");
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(assetPath);
            rootVisualElement.styleSheets.Add(styleSheet);

            var group = new GroupBox("Create");
            var mapNameField = new TextField("Map Name");
            group.Add(mapNameField);
            var createMapBtn = new Button();
            createMapBtn.text = "Create New Map";
            createMapBtn.clicked += () =>
            {
                try
                {
                    createMapBtn.SetEnabled(false);
                    VSMTCreateTools.CreateNewMap(mapNameField.value);
                }
                finally
                {
                    createMapBtn.SetEnabled(true);
                }
            };
            group.Add(createMapBtn);
            rootVisualElement.Add(group);

            group = new GroupBox("Lighting");
            var offsetField = new Vector3Field("Light Probe Offset");
            offsetField.value = new Vector3(0f, 0.25f, 0f);
            group.Add(offsetField);

            var areaSizeField = new Vector3Field("Light Probe Size");
            areaSizeField.value = new Vector3(20f, 5f, 30f);
            group.Add(areaSizeField);

            var spacingField = new FloatField("Light Probe Spacing");
            spacingField.value = 20f;
            group.Add(spacingField);

            var probeTestSizeField = new FloatField("Probe Test Size");
            probeTestSizeField.value = 0.25f;
            group.Add(probeTestSizeField);

            var generateLightProbesBtn = new Button();
            generateLightProbesBtn.text = "Generate Light Probes";
            generateLightProbesBtn.clicked += () =>
            {
                try
                {
                    generateLightProbesBtn.SetEnabled(false);
                    VSMTLightingTools.GenerateLightProbes(offsetField.value, areaSizeField.value, spacingField.value, probeTestSizeField.value);
                }
                finally
                {
                    generateLightProbesBtn.SetEnabled(true);
                }
            };
            group.Add(generateLightProbesBtn);
            rootVisualElement.Add(group);

            group = new GroupBox("Build");
            var buildBtn = new Button();
            buildBtn.text = "Build Currently Loaded Map";
            buildBtn.clicked += () =>
            {
                try
                {
                    buildBtn.SetEnabled(false);
                    VSMTBuildTools.BuildCurrentMap();
                }
                finally
                {
                    buildBtn.SetEnabled(true);
                }
            };
            group.Add(buildBtn);
            rootVisualElement.Add(group);

            group = new GroupBox("Play");
            _assetBundleScrollView = new ScrollView();
            group.Add(_assetBundleScrollView);
            rootVisualElement.Add(group);

            VSMTUtilities.OnAssetBundlesChanged += UpdateAssetBundleScrollView;
            VSMTBuildTools.BuildCompleted += (_) => UpdateAssetBundleScrollView();
        }

        private void UpdateAssetBundleScrollView()
        {
            if (_assetBundleScrollView == null)
            {
                return;
            }

            _assetBundleScrollView.Clear();
            foreach (var assetBundle in AssetDatabase.GetAllAssetBundleNames())
            {
                var mapPath = Path.Combine(Application.dataPath, "../AssetBundles", assetBundle);
                if (File.Exists(mapPath))
                {
                    var btn = new Button(() =>
                    {
                        VSMTUtilities.CopyAssetBundleToGameDir(assetBundle);
                    });
                    btn.text = $"Copy {assetBundle} to Mod Maps Directory";
                    _assetBundleScrollView.Add(btn);
                }
            }

            if (_assetBundleScrollView.childCount == 0)
            {
                _assetBundleScrollView.Add(new Label("No maps are currently built."));
            }
        }
    }
}
