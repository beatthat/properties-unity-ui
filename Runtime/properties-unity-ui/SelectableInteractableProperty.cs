using BeatThat.TransformPathExt;
using UnityEngine;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    /// <summary>
    /// Enables you to bind the UnnityEngine.UI.Selectable::interactable 
    /// property to a bool property
    /// </summary>
    [RequireComponent(typeof(Selectable))]
    public class SelectableInteractableProperty : BoolProp, IDrive<Selectable>
	{
		public override bool sendsValueObjChanged { get { return false; } }

        public Selectable m_selectable;

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

        public object GetDrivenObject()
        {
            return this.driven;
        }

        public bool ClearDriven()
        {
            m_selectable = null;
            return true;
        }

        protected override void EnsureValue(bool s)
        {
            this.driven.interactable = s;
        }

        protected override bool GetValue()
        {
            return this.driven.interactable;
        }

        protected override void _SetValue(bool s)
        {
            this.driven.interactable = s;
        }
    }
}

