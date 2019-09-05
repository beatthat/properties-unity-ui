using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    [RequireComponent(typeof(RawImage))]
    public class RawImageTexture : TextureProp, IDrive<RawImage>, IHasUVRect
    {
        #region IHasUVRect implementation
        public Rect uvRect { get { return this.driven.uvRect; } set { this.driven.uvRect = value; } }
        #endregion

        [FormerlySerializedAs("m_image")]private RawImage m_driven;

        public RawImage driven
        {
            get
            {
                return (m_driven != null) ? m_driven : (m_driven = GetComponent<RawImage>());
            }
        }

        protected override Texture GetValue()
        {
            var i = this.driven;
            return i != null ? i.texture : null;
        }

        protected override void _SetValue(Texture s)
        {
            var i = this.driven;
            if (i != null)
            {
                i.texture = s;
            }
        }


        protected override void EnsureValue(Texture v)
        {
            this.driven.texture = v;
        }

        public object GetDrivenObject()
        {
            return this.driven;
        }

        public bool ClearDriven()
        {
            m_driven = null;
            return true;
        }
    }
}

