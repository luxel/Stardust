namespace Stardust.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;

    public class StardustEditorMenus : MonoBehaviour
    {
        const string ResourceSimulationMode = Constants.EditorMenuPrefix + "/Resource Simulation Mode";

        [MenuItem(ResourceSimulationMode)]
        public static void ToggleSimulationMode()
        {
            StardustEditorSettings.ResourcesSimulationMode = !StardustEditorSettings.ResourcesSimulationMode;
        }

        [MenuItem(ResourceSimulationMode, true)]
        public static bool ToggleSimulationModeValidate()
        {
            Menu.SetChecked(ResourceSimulationMode, StardustEditorSettings.ResourcesSimulationMode);
            return true;
        }
        [MenuItem(Constants.EditorMenuPrefix + "/Build Asset Bundles")]
        static void BuildAssetBundles()
        {
            (new ResourceBuilder()).BuildAssetBundles();
        }

        [MenuItem(Constants.EditorMenuPrefix + "/Build Player")]
        static void BuildPlayer()
        {
            (new PlayerBuilder()).BuildPlayer();
        }
    }
}