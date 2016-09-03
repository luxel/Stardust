namespace Stardust.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    public class PlayerBuilder
    {
        /// <summary>
        /// The parent location to output the build. Usually selected by user.
        /// </summary>
        protected string outputPath;
        /// <summary>
        /// List of levels to include in the build.
        /// </summary>
        protected string[] levels;
        /// <summary>
        /// The target name for build
        /// </summary>
        protected string buildTargetName;
        /// <summary>
        /// The final full path passed to BuildPipeline.BuildPlayer
        /// </summary>
        protected string buildTargetPath;

        protected string LastUsedOutputPath
        {
            get
            {
                return EditorPrefs.GetString(EditorPrefKeys.BuildOutputPath, "");
            }
            set
            {
                EditorPrefs.SetString(EditorPrefKeys.BuildOutputPath, value);
            }
        }

        public void BuildPlayer()
        {
            SelectFolder();
            if (string.IsNullOrEmpty(outputPath))
            {
                Debug.LogError("[PlayerBuilder] No build output path is selected.");
                return;
            }

            DetermineBuildTargetName();
            if (string.IsNullOrEmpty(buildTargetName))
            {
                Debug.LogError("[PlayerBuilder] Empty build target name, the target platform is not supported!");
                return;
            }

            SelectLevels();
            Debug.Log(string.Format("[PlayerBuilder] Levels to build are: {0}", levels));

            DetermineBuildPath();
            Debug.Log(string.Format("[PlayerBuilder] build path is {0}", buildTargetPath));

            Debug.Log("[PlayerBuilder] Preparing build...");
            PrepareBuild();

            Debug.Log("[PlayerBuilder] Performing build...");
            DoBuild();

            Debug.Log("[PlayerBuilder] Build is completed successfully.");
        }        

        protected virtual void SelectFolder()
        {
            outputPath = EditorUtility.SaveFolderPanel("Choose Location of the Built Game", LastUsedOutputPath, "");

            if (!string.IsNullOrEmpty(outputPath))
            {
                LastUsedOutputPath = outputPath;
            }
        }

        protected virtual void SelectLevels()
        {
            levels = GetLevelsFromBuildSettings();
        }

        protected virtual void DetermineBuildTargetName()
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            string name = null;
            string format = null;
            switch (target)
            {
                case BuildTarget.Android:
                    name = "test.apk";
                    format = "{0}.apk";
                    break;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    name = "test.exe";
                    format = "{0}.exe";
                    break;
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                case BuildTarget.StandaloneOSXUniversal:
                    name = "test.app";
                    format = "{0}.app";
                    break;
                case BuildTarget.WebGL:
                case BuildTarget.iOS:
                    name = "";
                    break;
                // Add more build targets for your own.
                default:
                    Debug.Log("Target not implemented.");
                    break;
            }
            if (!string.IsNullOrEmpty(format))
            {
                name = string.Format(format, PlayerSettings.productName);
            }
            buildTargetName = name;
        }

        protected virtual void DetermineBuildPath()
        {
            buildTargetPath = Path.Combine(outputPath, buildTargetName);
        }

        protected virtual void PrepareBuild()
        {
            var builder = new ResourceBuilder();
            builder.BuildAssetBundles();
        }

        protected virtual void DoBuild()
        {
            BuildOptions option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
            BuildPipeline.BuildPlayer(levels, buildTargetPath, EditorUserBuildSettings.activeBuildTarget, option);
        }

        protected static string[] GetLevelsFromBuildSettings()
        {
            List<string> levels = new List<string>();
            for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i)
            {
                if (EditorBuildSettings.scenes[i].enabled)
                    levels.Add(EditorBuildSettings.scenes[i].path);
            }

            return levels.ToArray();
        }
    }
}