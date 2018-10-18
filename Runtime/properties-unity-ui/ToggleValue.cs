using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleValue : BoolProp, IDrive<Toggle>
	{
		public override bool sendsValueObjChanged { get { return false; } }

		public override object valueObj { get { return this.value; } }

        [FormerlySerializedAs("m_toggle")]public Toggle m_driven;

        public Toggle toggle 
        {
            get {
                return (m_driven != null) ?
                    m_driven : (m_driven = GetComponent<Toggle>());
            }
        }

        public Toggle driven { get { return this.toggle; } }

        private void OnValueChanged(bool v)
		{
			SendValueChanged (value);
		}

		override protected void Start()
		{
			base.Start ();
			this.toggle.onValueChanged.AddListener(this.OnValueChanged);
		}

        protected override bool GetValue()
        {
            return this.toggle.isOn;
        }

        protected override void _SetValue(bool v)
        {
            this.toggle.isOn = v;
        }

        protected override void EnsureValue(bool v)
        {
            this.toggle.isOn = v;
        }

        public object GetDrivenObject()
        {
            return this.driven;
        }

        public bool ClearDriven()
        {
            m_driven = null;
            return true;
        }
    }
}

