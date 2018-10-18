using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    /// <summary>
    /// Enables you to bind the UnityEngine.UI.Selectable::interactable 
    /// on a GameObject that doesn't directly include the target Selectable
    /// but instead has a component that implements IHasSelectable.
    /// A common example would be a controller that instantiates a view
    /// and the view contains the actual selectable, say a button.
    /// The controller can implement IHasSelectable to indicate it can provide
    /// the selectable.
    /// property to a bool property
    /// </summary>
    public class HasSelectableInteractableProperty : BoolProp, IDrive<IHasSelectable>
	{
		public override bool sendsValueObjChanged { get { return false; } }

        public IHasSelectable m_hasSelectable;

        public IHasSelectable driven
        {
            get
            {
                return (m_hasSelectable != null) ?
                    m_hasSelectable : (m_hasSelectable = GetComponent<IHasSelectable>());
            }
        }

        public object GetDrivenObject()
        {
            return this.driven;
        }

        public bool ClearDriven()
        {
            m_hasSelectable = null;
            return true;
        }

        public Selectable selectable 
        {
            get {
                var h = this.driven;
                return h != null ? h.selectable : null;
            }
        }

        protected override void EnsureValue(bool v)
        {
            var s = this.selectable;
            if(s != null) {
                s.interactable = v;
            }
        }

        protected override bool GetValue()
        {
            var s = this.selectable;
            return s != null ? s.interactable : false;
        }

        protected override void _SetValue(bool v)
        {
            var s = this.selectable;
            if (s != null)
            {
                s.interactable = v;
            }
        }
    }
}

