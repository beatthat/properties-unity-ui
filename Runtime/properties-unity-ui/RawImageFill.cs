using UnityEngine;

namespace BeatThat.Properties.UnityUI
{
    /// <summary>
    /// For using an raw image as a fill 
    /// </summary>
    [RequireComponent(typeof(FillableRawImage))]
	public class RawImageFill : HasFloat 
	{
		public FillableRawImage m_image;

		public override float value 
		{
			get {
				return this.image.fillAmount;
			}
			set {
				this.image.fillAmount = value;
			}
		}

		public override bool sendsValueObjChanged { get { return false; } }

		public FillableRawImage image { get { return m_image?? (m_image = GetComponent<FillableRawImage>()); } }

	}
}

