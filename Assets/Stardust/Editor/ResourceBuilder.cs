#if UNITY_EDITOR
namespace Stardust.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;
    using System.IO;

    public class ResourceBuilder
    {
        protected string outputPath;

        public void BuildAssetBundles()
        {
            Debug.Log("[ResourceBulider] Start building asset bundles...");
            DetermineOutputPath();

            Debug.Log(string.Format("[ResourceBuilder] Asset bundle output path is {0}", outputPath));
            PrepareOutputPath();

            Debug.Log("[ResourceBulider] Building...");
            PerformBuild();

            AssetDatabase.Refresh();
            Debug.Log("[ResourceBulider] Asset bundles built successfully...");
        }

        protected virtual void DetermineOutputPath()
        {
            outputPath = GameEnvironment.PacakgeResourcesPath;
        }

        protected virtual void PrepareOutputPath()
        {
            if (!Directory.Exists(outputPath))
            {
                Debug.Log("[ResourceBulider] Output path doesn't exist, creating...");
                Directory.CreateDirectory(outputPath);
            }
            FileUtility.ClearDirectory(outputPath);
        }

        protected virtual void PerformBuild()
        {
            var options = BuildAssetBundleOptions.UncompressedAssetBundle;
            BuildPipeline.BuildAssetBundles(outputPath, options, EditorUserBuildSettings.activeBuildTarget);
            //AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(outputPath, "characters"));
            //string[] assets = bundle.GetAllAssetNames();
            //Debug.Log(EditorUtilities.GetStringArray(assets));
        }
    }
} 
#endif