using BeatThat.TransformPathExt;
using UnityEngine;

namespace BeatThat.Properties.UnityUI
{
    public class CanvasGroupAlpha : DisplaysFloat 
	{
		public CanvasGroup m_canvasGroup;
		public bool m_invert;

		// Analysis disable ConvertToAutoProperty
		public bool invert { get { return m_invert; } set { m_invert = value; } }
		// Analysis restore ConvertToAutoProperty

		override public void UpdateDisplay()
		{
			var cg = this.canvasGroup;
			if (cg == null) {
				#if UNITY_EDITOR || DEBUG_UNSTRIP
				Debug.LogWarning("[" + Time.frameCount + "][" + this.Path() + "] " + GetType() + " - CanvasGroup is null. Maybe this behaviour is no longer wanted?");
				#endif
				return;
			}
			this.canvasGroup.alpha = (this.invert)? Mathf.Clamp01(1f - this.value): this.value;
		}

		// Analysis disable ConvertConditionalTernaryToNullCoalescing
		public CanvasGroup canvasGroup { get { return (m_canvasGroup != null)? m_canvasGroup: (m_canvasGroup = GetComponent<CanvasGroup>()); } }
		// Analysis restore ConvertConditionalTernaryToNullCoalescing

		#if UNITY_EDITOR
		void Reset()
		{
			m_canvasGroup = GetComponent<CanvasGroup>();
		}
		#endif

	}
}


