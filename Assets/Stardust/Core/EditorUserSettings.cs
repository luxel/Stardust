#if UNITY_EDITOR
namespace Stardust
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;

    public static class EditorStardustSettings
    {
        private static bool? _ResourcesSimulationMode;
        /// <summary>
        /// Whether the ResourceManager is under simulation mode.
        /// If true, resources will be directly loaded from asset path.
        /// </summary>
        public static bool ResourcesSimulationMode
        {
            get
            {
                if (!_ResourcesSimulationMode.HasValue)
                {
                    _ResourcesSimulationMode = EditorPrefs.GetBool(EditorPrefKeys.ResourceManagerSimulationMode, true);
                }
                return _ResourcesSimulationMode.Value;
            }
            set
            {
                if (_ResourcesSimulationMode.Value != value)
                {
                    _ResourcesSimulationMode = value;
                    EditorPrefs.SetBool(EditorPrefKeys.ResourceManagerSimulationMode, _ResourcesSimulationMode.Value);
                }
            }
        }
    }
}
#endif