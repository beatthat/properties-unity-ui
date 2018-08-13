using UnityEngine;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    [RequireComponent(typeof(RawImage))]
	public class RawImageTexture : HasTexture, IHasUVRect
	{
		#region IHasUVRect implementation
		public Rect uvRect { get { return this.image.uvRect; }  set { this.image.uvRect = value; } }
		#endregion

		private RawImage m_image;

		override protected Texture GetTexture() { return this.image.texture; }
		override protected void SetTexture(Texture t) { this.image.texture = t; }

		private RawImage image
		{
			get {
				if(m_image == null) {
					m_image = GetComponent<RawImage>();
				}
				return m_image;
			}
		}

	}
}

