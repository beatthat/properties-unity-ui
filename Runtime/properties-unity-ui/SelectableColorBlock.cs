using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    public class SelectableColorBlock : HasColorBlock 
	{
		public Selectable m_selectable;

		public override ColorBlock value 
		{
			get { return this.selectable.colors; }
			set { this.selectable.colors = value; }
		}

		public override bool sendsValueObjChanged { get { return false; } }
		public override object valueObj { get { return this.value; } }


		// Analysis disable ConvertConditionalTernaryToNullCoalescing
		public Selectable selectable { get { return (m_selectable != null)? m_selectable: (m_selectable = GetComponent<Selectable>()); } }
		// Analysis restore ConvertConditionalTernaryToNullCoalescing

		void Reset()
		{
			m_selectable = GetComponent<Selectable>();
		}
	}
}

