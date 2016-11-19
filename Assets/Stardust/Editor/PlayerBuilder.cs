#if UNITY_EDITOR
namespace Stardust.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class CustomPlayerBuilderAttribute : Attribute
    {
    }

    public class PlayerBuilder : CommonEditorTool
    {
        protected string versionString;
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
        /// <summary>
        /// The final path which holds the assets.
        /// </summary>
        protected string outputAssetsPath;

        protected bool appendVersionStringInOutputPath = true;

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

        protected string LastBuildTargetPath
        {
            get
            {
                return EditorPrefs.GetString(EditorPrefKeys.BuildLastTargetPath, null);
            }
            set
            {
                EditorPrefs.SetString(EditorPrefKeys.BuildLastTargetPath, value); 
            }
        }

        protected string LastBuildTargetName
        {
            get
            {
                return EditorPrefs.GetString(EditorPrefKeys.BuildLastTargetName, null);
            }
            set
            {
                EditorPrefs.SetString(EditorPrefKeys.BuildLastTargetName, value);
            }
        }

        public void BuildPlayer()
        {
            versionString = GenerateVersionString();
            Log("Version string is {0}", versionString);

            SelectFolder();

            DetermineBuildTargetName();
            if (string.IsNullOrEmpty(buildTargetName))
            {
                LogError("Empty build target name, the target platform is not supported!");
                return;
            }

            SelectLevels();
            Log("Levels to build are: {0}", levels);

            DetermineBuildPath();
			Log("build path is {0}", buildTargetPath);

            Log("Preparing build...");
            PrepareBuild();

            Log("Performing build...");
            DoBuild();

            Log("Generating build log...");
            CreateBuildLog();

            Log("Calling post process...");
            DoPostprocess();

            Log("Saving last build info...");
            LastBuildTargetPath = buildTargetPath;
            LastBuildTargetName = buildTargetName;

            Log("Build is completed successfully.");
        }

        public virtual void LaunchLastBuild()
        {
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    string lastBuildPath = LastBuildTargetPath;
                    if (!string.IsNullOrEmpty(lastBuildPath))
                    {                        
                        EditorUtilities.LaunchExternalProcess(lastBuildPath);
                    }
                    else
                    {
                        LogError("No build was executed yet. Please perform a build first.");
                    }
                    break;
                default:
                    LogError("[LaunchLastBuild] Build target {0} is not supported yet", EditorUserBuildSettings.activeBuildTarget);
                    break;
            }
        }

        protected virtual void SelectFolder()
        {
            outputPath = EditorUtility.SaveFolderPanel("Choose Location of the Built Game", LastUsedOutputPath, "");

            if (!string.IsNullOrEmpty(outputPath))
            {
                LastUsedOutputPath = outputPath;
            }
            if (string.IsNullOrEmpty(outputPath))
            {
                OnFatalError("No build output path is selected.");
            }
            if (appendVersionStringInOutputPath)
            {
                outputPath = Path.Combine(outputPath, versionString);
            }
        }

        protected virtual string GenerateVersionString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd_HH-mm");
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
                    OnFatalError("Build target not implemented.");
                    break;
            }
            if (!string.IsNullOrEmpty(format))
            {
                name = string.Format(format, GetProductBuildName());
            }
            buildTargetName = name;
        }

        /// <summary>
        /// Gets the platform independent build name (e.g. "Mercenary_Conflict")
        /// </summary>
        /// <returns></returns>
        protected virtual string GetProductBuildName()
        {
            return PlayerSettings.productName;
        }

        protected virtual void DetermineBuildPath()
        {
            buildTargetPath = Path.Combine(outputPath, buildTargetName);

            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
				outputAssetsPath = Path.Combine(outputPath, string.Format("{0}_Data", FileUtility.GetFileNameWithoutExtension(buildTargetName)));
                    break;
                case BuildTarget.Android:
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                case BuildTarget.StandaloneOSXUniversal:
                case BuildTarget.WebGL:
                case BuildTarget.iOS:
                // Add more build targets for your own.
                default:
                    OnFatalError("Build target not implemented.");
                    break;
            }
        }

        protected virtual void PrepareBuild()
        {
			bool isTargetPathDirectory = FileUtility.IsDirectory(buildTargetPath);

            bool needOverwrite = false;

            if (isTargetPathDirectory && Directory.Exists(buildTargetPath))
            {
                needOverwrite = true;
            }
            if (!isTargetPathDirectory && File.Exists(buildTargetPath))
            {
                needOverwrite = true;
            }

            if (needOverwrite)
            {
                bool canContinue = EditorUtility.DisplayDialog("Build output folder not empty",
                    string.Format("{0} already exists, builder is going to delete existing files. Continue?", buildTargetPath), "Yes", "No");
                if (!canContinue)
                {
                    OnFatalError("User cancelled the build");
                }

                CleanOutputPath();
            }

            var builder = EditorToolsFactory.CreateResourceBuilder();
            builder.BuildAssetBundles();
        }

        protected virtual void CleanOutputPath()
        {
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    if (Directory.Exists(outputAssetsPath))
                    {
						FileUtility.ClearDirectory(outputAssetsPath);                        
                    }
                    if (File.Exists(buildTargetPath))
                    {
                        File.Delete(buildTargetPath);
                    }
                    
                    break;
                default:
                    break;
            }
        }

        protected virtual void DoBuild()
        {
            BuildOptions option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
            BuildPipeline.BuildPlayer(levels, buildTargetPath, EditorUserBuildSettings.activeBuildTarget, option);
        }

        protected virtual void CreateBuildLog()
        {
            string buildLog = Path.Combine(outputPath, string.Format("{0}.txt", versionString));
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("Build time:\t").Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).AppendLine();
            builder.Append("Target Platform:\t").Append(EditorUserBuildSettings.activeBuildTarget).AppendLine();
            builder.Append("Build machine:\t").Append(System.Net.Dns.GetHostName()).AppendLine();
            File.WriteAllText(buildLog, builder.ToString(), System.Text.Encoding.UTF8);
        }

        protected virtual void DoPostprocess()
        {
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
#endif