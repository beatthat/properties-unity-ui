using UnityEngine;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    /// <summary>
    /// Enables you to bind the UnnityEngine.UI.Selectable::interactable 
    /// property to a bool property
    /// </summary>
    [RequireComponent(typeof(Selectable))]
    public class SelectableInteractableProperty : BoolProperty, IDrive<Selectable>
	{
		public override bool sendsValueObjChanged { get { return false; } }

		public override object valueObj { get { return this.value; } }

        public Selectable m_selectable;

		public override bool value 
		{
			get {
                return this.driven.interactable;
			}
			set {
                this.driven.interactable = value;
			}
		}

        public Selectable driven
        {
            get
            {
                if (m_selectable == null)
                {
                    m_selectable = GetComponent<Selectable>();
                }
                return m_selectable;
            }
        }

        private void OnValueChanged(bool value)
		{
			SendValueChanged (value);
		}

        public object GetDrivenObject()
        {
            return this.driven;
        }

        public bool ClearDriven()
        {
            m_selectable = null;
            return true;
        }
    }
}

