using UnityEngine;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    [RequireComponent(typeof(Slider))]
	public class SliderValue : EditsFloat 
	{
		public Slider m_slider;

		public override float value 
		{
			get {
				return this.slider.value;
			}
			set {
				this.slider.value = value;
			}
		}

		public override bool sendsValueObjChanged { get { return false; } }

		public override bool interactable {
			get {
				return this.slider.interactable;
			}
			set {
				this.slider.interactable = value;
			}
		}

		public Slider slider 
		{
			get {
				if(m_slider == null) {
					m_slider = GetComponent<Slider>();
				}
				return m_slider;
			}
		}

		private void OnValueChanged(float value)
		{
			SendValueChanged();
		}

		override protected void Start()
		{
			base.Start ();
			this.slider.onValueChanged.AddListener(this.OnValueChanged);
		}

	}
}

