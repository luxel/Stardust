#if UNITY_EDITOR
namespace Stardust.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections;
    using System.Reflection;

    public static class EditorToolsFactory
    {
        public static PlayerBuilder CreatePlayerBuilder()
        {
            Type[] types = EditorUtilities.FindTypesFromAllAssembliesWithAttribute<CustomPlayerBuilderAttribute>(typeof(PlayerBuilder));

            if (types == null || types.Length == 0)
            {
                return new PlayerBuilder();
            }
            else
            {                
                return Activator.CreateInstance(types[0]) as PlayerBuilder;
            }
        }

        public static PlayerBuilder CreateMockTestBuilder()
        {
            return new MockTestBuilder();
        }

        public static ResourceBuilder CreateResourceBuilder()
        {
            return new ResourceBuilder();
        }
    }

} 
#endif