using BeatThat.Properties;
using UnityEngine;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
	[RequireComponent(typeof(Toggle))]
	public class ToggleValue : BoolProperty 
	{
		public override bool sendsValueObjChanged { get { return false; } }

		public override object valueObj { get { return this.value; } }

		public Toggle m_toggle;


		public override bool value 
		{
			get {
				return this.toggle.isOn;
			}
			set {
				this.toggle.isOn = value;
			}
		}


		public Toggle toggle
		{
			get {
				if(m_toggle == null) {
					m_toggle = GetComponent<Toggle>();
				}
				return m_toggle;
			}
		}

		private void OnValueChanged(bool value)
		{
			SendValueChanged (value);
		}

		override protected void Start()
		{
			base.Start ();
			this.toggle.onValueChanged.AddListener(this.OnValueChanged);
		}

	}
}

