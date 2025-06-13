using UnityEditor;


namespace VirtualSkateMappingTools
{
    public static class VSMTMenuItems
    {
        [MenuItem("Tools/Open Virtual Skate Mapping Tools")]
        public static void OpenVSMTWindowMenuItem()
        {
            VSMTEditorWindow.OpenWindow();
        }
    }
}
