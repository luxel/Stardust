#if UNITY_EDITOR
namespace Stardust.Editor
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.IO;
    using UnityEditor;

    public static class EditorUtilities
    {
        public static bool LaunchExternalProcess(string fileName, string arguments = null)
        {
            Debug.LogFormat("Launching {0} {1}", fileName, string.IsNullOrEmpty(arguments)? string.Empty : arguments);

            if (!File.Exists(fileName))
            {
                Debug.LogErrorFormat("File {0} doesn't exist!", fileName);
                return false;
            }

            System.Diagnostics.Process myProcess = new  System.Diagnostics.Process();
            myProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            myProcess.StartInfo.CreateNoWindow = false;
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.FileName = fileName;
            myProcess.StartInfo.Arguments = arguments;
            return myProcess.Start();            
        }
        public static bool IsInMacOS
        {
            get
            {
                return UnityEngine.SystemInfo.operatingSystem.IndexOf("Mac OS") != -1;
            }
        }

        public static bool IsInWinOS
        {
            get
            {
                return UnityEngine.SystemInfo.operatingSystem.IndexOf("Windows") != -1;
            }
        }
        
        public static void OpenInMac(string path)
        {
            bool openInsidesOfFolder = false;

            // try mac
            string macPath = path.Replace("\\", "/"); // mac finder doesn't like backward slashes

            if (System.IO.Directory.Exists(macPath)) // if path requested is a folder, automatically open insides of that folder
            {
                openInsidesOfFolder = true;
            }

            if (!macPath.StartsWith("\""))
            {
                macPath = "\"" + macPath;
            }

            if (!macPath.EndsWith("\""))
            {
                macPath = macPath + "\"";
            }

            string arguments = (openInsidesOfFolder ? "" : "-R ") + macPath;

            try
            {
                System.Diagnostics.Process.Start("open", arguments);
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                // tried to open mac finder in windows
                // just silently skip error
                // we currently have no platform define for the current OS we are in, so we resort to this
                e.HelpLink = ""; // do anything with this variable to silence warning about not using it
            }
        }

        public static void OpenInWin(string path)
        {
            bool openInsidesOfFolder = false;

            // try windows
            string winPath = path.Replace("/", "\\"); // windows explorer doesn't like forward slashes

            if (System.IO.Directory.Exists(winPath)) // if path requested is a folder, automatically open insides of that folder
            {
                openInsidesOfFolder = true;
            }

            try
            {
                System.Diagnostics.Process.Start("explorer.exe", (openInsidesOfFolder ? "/root," : "/select,") + winPath);
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                // tried to open win explorer in mac
                // just silently skip error
                // we currently have no platform define for the current OS we are in, so we resort to this
                e.HelpLink = ""; // do anything with this variable to silence warning about not using it
            }
        }

        public static void Open(string path)
        {
            if (IsInWinOS)
            {
                OpenInWin(path);
            }
            else if (IsInMacOS)
            {
                OpenInMac(path);
            }
            else // couldn't determine OS
            {
                OpenInWin(path);
                OpenInMac(path);
            }
        }
        public static string GetStringArray(string[] array)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                builder.AppendLine(array[i]);
            }
            return builder.ToString();
        }

        public static string DumpDictionary(Dictionary<string, string> dict)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            foreach (var key in dict.Keys)
            {
                builder.Append(key).Append('=').Append(dict[key]);
            }
            return builder.ToString();
        }

        public static string DumpBytes(byte[] data)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            for (int i = 0; i < data.Length; i ++)
            {
                builder.Append(data[i]).Append(',');                
            }
            return builder.ToString();
        }

        public static byte[] CharsToBytes(char[] data)
        {
            byte[] bytes = new byte[data.Length];
            for (int i = 0; i < data.Length; i ++)
            {
                bytes[i] = (byte)data[i];
            }
            return bytes;
        }

        public static string[] GetAllPrefabsWithComponent<T>() where T: Component
        {
            string[] prefabs = GetAllPrefabs();
            List<string> result = new List<string>();
            foreach (string prefab in prefabs)
            {
                UnityEngine.Object o = AssetDatabase.LoadMainAssetAtPath(prefab);
                try
                {
                    GameObject go = (GameObject)o;
                    if (go != null)
                    {
                        Component[] components = go.GetComponents<T>();
                        if (components != null && components.Length > 0)
                        {
                            result.Add(prefab);
                        }
                    }
                }
                catch(InvalidCastException)
                {
                    Debug.LogErrorFormat("Object {0} unable to cast to GameObject", o.name);
                }
            }
            return result.ToArray();
        }

        public static string[] GetAllPrefabs()
        {
            string[] temp = AssetDatabase.GetAllAssetPaths();
            List<string> result = new List<string>();
            foreach (string s in temp)
            {
                if (s.Contains(".prefab")) result.Add(s);
            }
            return result.ToArray();
        }

        /// <summary>
        /// Find types with specified attribute, whild also child classes of baseType
        /// This should only be used under editor because it's slow.
        /// </summary>
        /// <typeparam name="A">The attribute to search.</typeparam>
        /// <param name="baseType">The base class type to search</param>
        /// <returns></returns>
        public static Type[] FindTypesFromAllAssembliesWithAttribute<A>(Type baseType)
            where A: System.Attribute
        {
            List<Type> targetTypes = new List<Type>();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            Type targetAttribute = typeof(A);

            for (int i = 0; i < assemblies.Length; i ++)
            {
                Type[] types = assemblies[i].GetTypes();
                for (int t = 0; t < types.Length; t ++)
                {
                    if (types[t].IsDefined(targetAttribute, true) && 
                        (baseType == null || types[t].IsSubclassOf(baseType)))
                    {
                        targetTypes.Add(types[t]);
                    }
                }
            }

            return targetTypes.ToArray();
        }        

        public static byte[] ReadDataFile(string name)
        {
            string filePath = EditorEnvironment.GetEditorDataFile(name);
            return File.ReadAllBytes(filePath);
        }

        public static void WriteDataFile(string text, string name)
        {
            string filePath = EditorEnvironment.GetEditorDataFile(name);
            char[] chars = text.ToCharArray();
            File.WriteAllBytes(filePath, CharsToBytes(chars));
        }

        public static void WriteDataFileWithPath(string text, string path)
        {
            char[] chars = text.ToCharArray();
            File.WriteAllBytes(path, CharsToBytes(chars));
        }
    }
} 
#endif
