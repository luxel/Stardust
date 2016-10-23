namespace Stardust
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Manages all UI atlases during runtime.
    /// </summary>
    public static class UIAtlasManager
    {
        private static Dictionary<string, UIAtlas> _Atlases = new Dictionary<string, UIAtlas>();

        public static void RegisterAtlas(UIAtlas atlas)
        {
            string name = atlas.AtlasName;
            if (!_Atlases.ContainsKey(name))
            {
                _Atlases.Add(name, atlas);
            }
        }

        public static UIAtlas FindAtlas(string atlasName)
        {
            UIAtlas atlas = null;
            _Atlases.TryGetValue(atlasName, out atlas);
            if (atlas == null)
            {
                Debug.LogErrorFormat("No atlas is found for {0}", atlasName);
            }
            return atlas;
        }

        public static Sprite FindSprite(string atlasName, string spriteName)
        {
            UIAtlas atlas = FindAtlas(atlasName);
            if (atlas != null)
            {
                return atlas.FindSpriteByName(spriteName);
            }
            else
            {
                return null;
            }
        }
    }
}