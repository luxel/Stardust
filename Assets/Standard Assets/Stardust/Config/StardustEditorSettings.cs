#if UNITY_EDITOR
namespace Stardust
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;

    public static class StardustEditorSettings
    {
        /// <summary>
        /// Whether the ResourceManager is under simulation mode.
        /// If true, resources will be directly loaded from asset path.
        /// </summary>
        public static bool ResourcesSimulationMode
        {
            get
            {
                return EditorPrefs.GetBool(EditorPrefKeys.ResourceManagerSimulationMode, true);
            }
            set
            {
                EditorPrefs.SetBool(EditorPrefKeys.ResourceManagerSimulationMode, false);
            }
        }

    }
}
#endif