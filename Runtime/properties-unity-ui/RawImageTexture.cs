using UnityEngine;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    [RequireComponent(typeof(RawImage))]
    public class RawImageTexture : HasTexture, IHasUVRect
    {
        #region IHasUVRect implementation
        public Rect uvRect { get { return this.image.uvRect; } set { this.image.uvRect = value; } }
        #endregion

        private RawImage m_image;

        override public Texture value
        {
            get
            {
                var i = this.image;
                return i != null ? i.texture : null;
            }
            set
            {
                var i = this.image;
                if(i != null)
                {
                    i.texture = value;
                }
            }
        }

        private RawImage image
        {
            get
            {
                if (m_image == null)
                {
                    m_image = GetComponent<RawImage>();
                }
                return m_image;
            }
        }

    }
}

