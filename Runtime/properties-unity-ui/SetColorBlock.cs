using BeatThat.ColorAssets;
using BeatThat.TransformPathExt;
using UnityEngine;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    /// <summary>
    /// Set the color on 
    /// Exposes the IHasFloat set_value interface, so this component can be used more easily in transitions (e.g. as an element of a TransitionsElements)
    /// </summary>
    public class SetColorBlock : MonoBehaviour, IDrive<HasColorBlock>
	{
		[SerializeField]private HasColorBlock m_hasColorBlock;
		[SerializeField]private ColorBlockAsset m_colorBlockAsset;
		[SerializeField]private ColorBlock m_colorBlock;
		[SerializeField]private bool m_useAsset;

		// Analysis disable ConvertConditionalTernaryToNullCoalescing
		public HasColorBlock driven { get { return (m_hasColorBlock!=null)? m_hasColorBlock: (m_hasColorBlock = GetComponent<HasColorBlock>()); } }
		// Analysis restore ConvertConditionalTernaryToNullCoalescing

		public object GetDrivenObject() { return this.driven; }

		public ColorBlock colorBlock { get { return m_useAsset && m_colorBlockAsset != null? m_colorBlockAsset.colors: m_colorBlock; } }

		void OnEnable()
		{
			UpdateDisplay();
		}

		public bool ClearDriven ()
		{
			m_hasColorBlock = null; return true;
		}

		public void UpdateDisplay()
		{
			var d = this.driven;
			if(d == null) {
				if(!Application.isPlaying) {
					return;
				}
				#if BT_DEBUG_UNSTRIP || UNITY_EDITOR
				Debug.LogWarning("[" + Time.frameCount + "][" + this.Path() + "] driven HasColor for SetColor is not set");
				#endif
				return;
			}
			d.value = this.colorBlock;
		}

		void OnDidApplyAnimationProperties()
		{
			UpdateDisplay();
		}

		#if UNITY_EDITOR
		void Reset()
		{
			m_hasColorBlock = GetComponent<HasColorBlock>();

			if(m_hasColorBlock == null && GetComponent<Selectable>() != null) {
				m_hasColorBlock = this.gameObject.AddComponent<SelectableColorBlock>();
			}
		}
		#endif
			
	}
}


