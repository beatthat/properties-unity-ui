using UnityEngine;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    [RequireComponent(typeof(Image))]
    public class ImageSprite : SpriteProp, IDrive<Image>
    {
        public override bool sendsValueObjChanged { get { return false; } }

        public override object valueObj { get { return this.value; } }

        public Image m_driven;

        public Image image
        {
            get
            {
                return (m_driven != null) ?
                    m_driven : (m_driven = GetComponent<Image>());
            }
        }

        public Image driven { get { return this.image; } }

        private void OnValueChanged(Sprite v)
        {
            SendValueChanged(value);
        }

        protected override Sprite GetValue()
        {
            return this.image.sprite;
        }

        protected override void _SetValue(Sprite v)
        {
            this.image.sprite = v;
        }

        protected override void EnsureValue(Sprite v)
        {
            this.image.sprite = v;
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

