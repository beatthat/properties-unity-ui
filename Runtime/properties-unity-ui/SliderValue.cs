using UnityEngine;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    [RequireComponent(typeof(Slider))]
    public class SliderValue : FloatProp 
	{
		public Slider m_slider;

        override protected void EnsureValue(float s) 
        {
            this.slider.value = s;
        }

        protected override float GetValue()
        {
            return this.slider.value;
        }

        protected override void _SetValue(float s)
        {
            this.slider.value = s;
        }

		public override bool sendsValueObjChanged { get { return true; } }

		public bool interactable {
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
            SendValueChanged(value);
		}

		override protected void Start()
		{
			base.Start ();
			this.slider.onValueChanged.AddListener(this.OnValueChanged);
		}

    }
}

