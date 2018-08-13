using UnityEngine;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    public class GraphicMaterial : HasMaterial 
	{
		public Graphic m_graphic;

		#region implemented abstract members of HasFloat

		public override Material value 
		{
			get {
				return this.graphic.material;
			}
			set {
				this.graphic.material = value;
			}
		}

		#endregion

		public Graphic graphic { get { return m_graphic?? (m_graphic = GetComponent<Graphic>()); } }

	}
}

