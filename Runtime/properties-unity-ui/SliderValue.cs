using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    [RequireComponent(typeof(Slider))]
    public class SliderValue : FloatProp, IDrive<Slider> 
	{
        [FormerlySerializedAs("m_slider")]public Slider m_driven;

		public override bool sendsValueObjChanged { get { return true; } }

		public bool interactable {
			get {
				return this.slider.interactable;
			}
			set {
				this.slider.interactable = value;
			}
		}

        public Slider slider { get { return this.driven; } }

        public Slider driven { get { return (m_driven != null) ? m_driven : (m_driven = GetComponent<Slider>()); } }

        private void OnValueChanged(float v)
		{
			SendValueChanged(v);
		}

		override protected void Start()
		{
			base.Start ();
			this.slider.onValueChanged.AddListener(this.OnValueChanged);
		}

        public object GetDrivenObject()
        {
            return this.driven;
        }

        public bool ClearDriven()
        {
            m_driven = null;
            return false;
        }

        protected override float GetValue()
        {
            var d = this.driven;
            return (d != null) ? d.value : 0f;
        }

        protected override void _SetValue(float v)
        {
            var d = this.driven;
            if(d != null) {
                d.value = v;
            }
        }

        protected override void EnsureValue(float v)
        {
            var d = this.driven;
            if (d != null)
            {
                d.value = v;
            }
        }
    }
}

