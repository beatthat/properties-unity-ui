using UnityEngine;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    /// <summary>
    /// For using an image as a fill
    /// </summary>
    [RequireComponent(typeof(Image))]
	public class ImageFill : HasFloat, IDrive<Image>
	{
		public Image m_image;

		public Image driven { get { return this.image; } }

		public override float value 
		{
			get {
				return this.image.fillAmount;
			}
			set {
				if(Mathf.Approximately(value, this.image.fillAmount)) {
					return;
				}
				this.image.fillAmount = value;
			}
		}

		public override bool sendsValueObjChanged { get { return false; } }

		public Image image { get { return m_image?? (m_image = GetComponent<Image>()); } }

		public object GetDrivenObject ()
		{
			return this.driven;
		}

		public bool ClearDriven ()
		{
			m_image = null;
			return true;
		}



	}
}

