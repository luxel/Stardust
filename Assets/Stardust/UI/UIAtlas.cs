namespace Stardust
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Records all sprites information for one atlas, so later the sprites could be easily located during runtime.
    /// </summary>
    public class UIAtlas : MonoBehaviour
    {
        /// <summary>
        /// Name of the atlas. If empty, the game object name will be used.
        /// </summary>
        [SerializeField]
        public string AtlasName;

        /// <summary>
        /// Collection of sprites in this atlas.
        /// </summary>
        [SerializeField]
        public Sprite[] Sprites;
        /// <summary>
        /// An internal dictionary storing all sprites for quick searching.
        /// </summary>
        private Dictionary<string, Sprite> spritesCollection;

        void Awake()
        {
            if (string.IsNullOrEmpty(AtlasName))
            {
                AtlasName = gameObject.name;
            }
            CheckAndBuildSpriteCollection();
        }

        /// <summary>
        /// Find a sprite instance with its name.
        /// </summary>
        public Sprite FindSpriteByName(string name)
        {
            CheckAndBuildSpriteCollection();
            Sprite sprite = null;
            spritesCollection.TryGetValue(name, out sprite);
            return sprite;
        }

        private void CheckAndBuildSpriteCollection()
        {
            if (spritesCollection == null)
            {
                BuildSpriteCollection();
            }
        }

        private void BuildSpriteCollection()
        {
            spritesCollection = new Dictionary<string, Sprite>();
            for (int i = 0; i < Sprites.Length; i++)
            {
                spritesCollection[Sprites[i].name] = Sprites[i];
            }
        }
    }
}