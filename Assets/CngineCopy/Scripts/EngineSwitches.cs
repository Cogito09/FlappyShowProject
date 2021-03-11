#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Cngine
{
    public static class EngineSwitches
    {
        [MenuItem("Cngine/EngineSwitches/IsFirebaseEnabled", true)]
        private static bool IsFirebaseEnabled()
        {
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android).Contains("USE_FIREBASE") == false;
        }

        [MenuItem("Cngine/EngineSwitches/IsFirebaseDisabled", true)]
        private static bool IsFirebaseDisabled()
        {
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android).Contains("USE_FIREBASE");
        }
        
        [MenuItem("Cngine/EngineSwitches/Firebase/On")]
        public static void EnableFirebase()
        {
            PlayerPrefs.DeleteAll();
            var testingGroup = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
            var testingGroupiOS = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, $"{testingGroup};USE_FIREBASE");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, $"{testingGroupiOS};USE_FIREBASE");
            
            AssetDatabase.SaveAssets();
            EditorApplication.ExecuteMenuItem("File/Save Project");
        }

        [MenuItem("Cngine/EngineSwitches/Firebase/Off")]
        public static void DisableFirebase()
        {
            PlayerPrefs.DeleteAll();
            var testingGroup = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
            var testingGroupiOS = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, testingGroup.Replace(";USE_FIREBASE", ""));
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, testingGroupiOS.Replace(";USE_FIREBASE", ""));

            AssetDatabase.SaveAssets();
            EditorApplication.ExecuteMenuItem("File/Save Project");
        }
    }
}
#endif