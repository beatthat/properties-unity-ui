using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    /// <summary>
    /// For using an image as a fill
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ImageFill : FloatProp, IDrive<Image>
	{
        [FormerlySerializedAs("m_image")]public Image m_driven;
        public override bool sendsValueObjChanged { get { return false; } }

        public override object valueObj { get { return this.value; } }

        public Image driven { get { return m_driven ?? (m_driven = GetComponent<Image>()); } }

        public Image image { get { return this.driven; } }

		public object GetDrivenObject ()
		{
			return this.driven;
		}

		public bool ClearDriven ()
		{
			m_driven = null;
			return true;
		}

        protected override float GetValue()
        {
            return this.image.fillAmount;
        }

        protected override void _SetValue(float v)
        {
            this.image.fillAmount = v;
        }

        protected override void EnsureValue(float v)
        {
            this.image.fillAmount = v;
        }
    }
}

