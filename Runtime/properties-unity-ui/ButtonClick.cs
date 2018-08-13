using UnityEngine;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    [RequireComponent(typeof(Button))]
	public class ButtonClick : HasClick 
	{
		public Button m_button;

		#if LEGACY_HAS_CLICK
		public override bool interactable 
		{
			get {
				return this.button.interactable;
			}
			set {
				this.button.interactable = value;
			}
		}

		#endif

		public Button button { get { return m_button != null ? m_button : (m_button = GetComponent<Button> ()); } }

		void Start()
		{
			this.button.onClick.AddListener(this.SendClickEvent);
		}

	}
}

