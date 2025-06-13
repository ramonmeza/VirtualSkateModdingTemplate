using System.IO;
using UnityEditor;
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

            group = new GroupBox("Build");
            var buildBtn = new Button();
            buildBtn.text = "Build Currently Loaded Map";
            buildBtn.clicked += () =>
            {
                buildBtn.SetEnabled(false);
                try
                {
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
            EditorApplication.projectChanged += UpdateAssetBundleScrollView;
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
                if (!File.Exists(mapPath))
                {
                    continue;
                }

                var btn = new Button(() =>
                {
                    VSMTUtilities.CopyAssetBundleToGameDir(assetBundle);
                });
                btn.text = $"Copy {assetBundle} to Mod Maps Directory";
                _assetBundleScrollView.Add(btn);
            }

            Repaint();
        }
    }
}
