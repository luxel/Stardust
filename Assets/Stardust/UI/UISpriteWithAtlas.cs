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
        [SerializeField]
        private SpriteRenderer spriteRenderer = null;
        [SerializeField]
        private string spriteName = null;

        public string SpriteName
        {
            get
            {
                return spriteName;
            }
        }

        void Awake()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
            if (!string.IsNullOrEmpty(spriteName))
            {
                SetSprite(spriteName);
            }
        }

        public void SetSprite(string spriteName)
        {
            Sprite sprite = UIAtlasManager.FindSprite(AtlasName, spriteName);
            this.spriteName = spriteName;

            if (sprite != null)
            {
                spriteRenderer.sprite = sprite;
                if (!spriteRenderer.enabled)
                {
                    spriteRenderer.enabled = true;
                }
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
    }
}