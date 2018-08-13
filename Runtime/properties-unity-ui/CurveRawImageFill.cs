using UnityEngine;

namespace BeatThat.Properties.UnityUI
{
    /// <summary>
    /// For using an raw image as a fill with a non linear curve
    /// </summary>
    [RequireComponent(typeof(FillableRawImage))]
	public class CurveRawImageFill : HasFloat 
	{
		public FillableRawImage m_image;
		public bool m_useCurve = true;
		public AnimationCurve m_curve;
		public float m_value;

		void Reset()
		{
			if(m_curve == null || m_curve.length == 0) {
				m_curve = AnimationCurve.Linear(0, 0, 1, 1);
			}
		}

		public override float value 
		{
			get {
				return m_value;
			}
			set {
				m_value = value;
				UpdateFill();
			}
		}

		public override bool sendsValueObjChanged { get { return false; } }

		void UpdateFill()
		{
			var fill = m_useCurve? m_curve.Evaluate(this.value): this.value;
			this.image.fillAmount = fill;
		}

		public FillableRawImage image { get { return m_image?? (m_image = GetComponent<FillableRawImage>()); } }
	}
}

