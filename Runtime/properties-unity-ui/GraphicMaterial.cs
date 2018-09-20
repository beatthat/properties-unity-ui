using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    public class GraphicMaterial : HasMaterial, IDrive<Graphic>
	{
		[FormerlySerializedAs("m_graphic")]public Graphic m_driven;

		public override Material value 
		{
			get {
                return this.driven.material;
			}
			set {
                this.driven.material = value;
			}
		}

        public Graphic graphic { get { return this.driven; } }

        public Graphic driven { get { return m_driven ?? (m_driven = GetComponent<Graphic>()); } }
    
        public bool ClearDriven()
        {
            m_driven = null;
            return true;
        }

        public object GetDrivenObject()
        {
            return this.driven;
        }
    }
}

