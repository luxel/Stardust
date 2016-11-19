#if UNITY_EDITOR
namespace Stardust.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;

    public class EditorMenus : MonoBehaviour
    {
        const string ResourceSimulationMode = "Stardust/Resource Simulation Mode";

        [MenuItem(ResourceSimulationMode)]
        public static void ToggleSimulationMode()
        {
			EditorStardustSettings.ResourcesSimulationMode = !EditorStardustSettings.ResourcesSimulationMode;
        }
		
		[MenuItem(ResourceSimulationMode, true)]
        public static bool ToggleSimulationModeValidate()
        {
			Menu.SetChecked(ResourceSimulationMode, EditorStardustSettings.ResourcesSimulationMode);
            return true;
        }

        [MenuItem("Stardust/Clear Local Data")]
        static void ClearLocalData()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        #region Build utilities

		[MenuItem("Stardust/Build Utilities/Build Asset Bundles")]
        static void BuildAssetBundles()
        {
            EditorToolsFactory.CreateResourceBuilder().BuildAssetBundles();
        }

		[MenuItem("Stardust/Build Utilities/Build Player")]
        static void BuildPlayer()
        {
            EditorToolsFactory.CreatePlayerBuilder().BuildPlayer();
        }

		[MenuItem("Stardust/Build Utilities/Mock Test")]
        static void MockTestBuildPlayer()
        {
            EditorToolsFactory.CreateMockTestBuilder().BuildPlayer();
        }

		[MenuItem("Stardust/Build Utilities/Launch Latest Build %l")]
        static void LaunchLatestBuild()
        {
            EditorToolsFactory.CreatePlayerBuilder().LaunchLastBuild();
        }

        #endregion

		[MenuItem("Stardust/Write binary data file from chars")]
        static void WriteBinaryFileFromChars()
        {
            string path = EditorUtility.SaveFilePanel("", EditorEnvironment.EditorDataFilePath, "", EditorConstants.DataFileExtension);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            SimpleInputDialog dialog = ScriptableObject.CreateInstance<SimpleInputDialog>();
            dialog.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
            dialog.CustomData = path;
            dialog.OkAction = new System.Action<SimpleInputDialog>(WriteBinaryFile);
            dialog.Show(true);
        }
        static void WriteBinaryFile(SimpleInputDialog dialog)
        {
            EditorUtilities.WriteDataFileWithPath(dialog.Value, dialog.CustomData as string);
            AssetDatabase.Refresh();
            Debug.LogFormat("String \"{0}\" saved to file {1}", dialog.Value, dialog.CustomData as string);
        }
    }
} 
#endif