#if UNITY_EDITOR
namespace Stardust.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;
    using System.IO;

    public class PlayerBuilderWithEncryption : PlayerBuilder
    {
        protected const string AssemblyName = "Assembly-CSharp.dll";
        /// <summary>
        /// Relative path under build output folder for Assembly-CSharp.dll
        /// </summary>
        protected static string AssemblyRelativePath = Path.Combine("Managed", AssemblyName);
        protected const string MonoName = "mono.dll";
        /// <summary>
        /// Relative path under build output folder for mono.dll
        /// </summary>
        protected static string MonoRelativePath = Path.Combine("Mono", MonoName);

        protected string targetAssemblyPath = null;
        protected string targetMonoPath = null;
        protected string replacingMonoPath = null;

        protected override void PrepareBuild()
        {
            base.PrepareBuild();
            
            // Check the source mono dll.
            string folder = EditorEnvironment.GetPlatformSpecificAssetPathForCurrentUnity("mono");
            replacingMonoPath = Path.Combine(folder, MonoName);
            CheckFileExistence(replacingMonoPath);
        }

        protected override void DoPostprocess()
        {
            // 1. Encrypt Assembly-CSharp.dll
            EncryptAssembly();

            // 2. Replace mono.dll
            ReplaceMono();
        }

        protected void EncryptAssembly()
        {
            targetAssemblyPath = Path.Combine(outputAssetsPath, AssemblyRelativePath);
            CheckFileExistence(targetAssemblyPath);
            Log("Encrypting {0}", targetAssemblyPath);

            // TODO - the actual encryption.
            string tempFile = targetAssemblyPath + ".temp";

            Log("Generating temp assembly file...");
            byte[] data = File.ReadAllBytes(targetAssemblyPath);
            byte[] key = EditorUtilities.ReadDataFile("assembly_encrption_key");
            byte[] output = Xxtea.XXTEA.Encrypt(data, key);
            Log("Encryption key is {0}", EditorUtilities.DumpBytes(key));
            Log("Data length before encryption: {0} after encryption: {1}", data.Length, output.Length);
            File.WriteAllBytes(tempFile, output);

            Log("Replacing {0} with {1}...", targetAssemblyPath, tempFile);
            File.Copy(tempFile, targetAssemblyPath, true);

            Log("Removing temp file...");
            File.Delete(tempFile);
        }

        protected void ReplaceMono()
        {
            targetMonoPath = Path.Combine(outputAssetsPath, MonoRelativePath);
            CheckFileExistence(targetMonoPath);           

            Log("Replacing {0} with {1}", targetMonoPath, replacingMonoPath);
                        
            File.Copy(replacingMonoPath, targetMonoPath, true);
        }
    }

} 
#endif