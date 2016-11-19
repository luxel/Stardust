namespace Stardust
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    /// <summary>
    /// Used with SpriteRenderer, used to programatically change the sprite.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class UISpriteWithAtlas : MonoBehaviour
    {
        /// <summary>
        /// Name of atlas.
        /// </summary>
        [SerializeField]
        public string AtlasName;   

        private SpriteRenderer renderer = null;

        void Awake()
        {
            if (renderer == null)
            {
                renderer = GetComponent<SpriteRenderer>();
            }
        }

        public void SetSprite(string spriteName)
        {
            renderer.sprite = UIAtlasManager.FindSprite(AtlasName, spriteName);
        }
    }
}