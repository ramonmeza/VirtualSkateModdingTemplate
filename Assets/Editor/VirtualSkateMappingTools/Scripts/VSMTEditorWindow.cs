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

            var group = new GroupBox("Build");
            var buildBtn = new Button();
            buildBtn.text = "Build Currently Loaded Map";
            buildBtn.clicked += () =>
            {
                buildBtn.SetEnabled(false);
                VSMTBuildTools.BuildCurrentMap();
                buildBtn.SetEnabled(true);
            };
            group.Add(buildBtn);
            rootVisualElement.Add(group);

            group = new GroupBox("Copy to Mod Maps Directory");
            _assetBundleScrollView = new ScrollView();
            group.Add(_assetBundleScrollView);
            rootVisualElement.Add(group);

            VSMTUtilities.OnAssetBundlesChanged += UpdateAssetBundleScrollView;
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
                var btn = new Button(() =>
                {
                    VSMTUtilities.CopyAssetBundleToGameDir(assetBundle);
                });
                btn.text = assetBundle;
                _assetBundleScrollView.Add(btn);
            }
        }
    }
}
