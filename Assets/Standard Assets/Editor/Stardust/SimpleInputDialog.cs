#if UNITY_EDITOR
namespace Stardust.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections;

    public class SimpleInputDialog : EditorWindow
    {
        public string Value { get; private set; }
        /// <summary>
        /// Custom data which can be used on the dialog.
        /// </summary>
        public object CustomData;

        public Action<SimpleInputDialog> OkAction;
        public Action<SimpleInputDialog> CancelAction;

        void OnGUI()
        {
            Value = EditorGUILayout.TextField("Please input", Value);
            if (GUILayout.Button("OK"))
            {
                this.Close();
                if (OkAction != null)
                {
                    OkAction.Invoke(this);
                    OkAction = null;
                }
            }
            if (GUILayout.Button("Cancel"))
            {
                this.Close();
                if (CancelAction != null)
                {
                    CancelAction.Invoke(this);
                    CancelAction = null;
                }
            }
        }
    }
} 
#endif