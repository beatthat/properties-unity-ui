using System;
using UnityEngine;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    /// <summary>
    /// Extended version of Unity's UI.RawImage that includes all the fill options of UI.Image
    /// </summary>
    [AddComponentMenu("UI/Ext/FillableRawImage", 12)]
	public class FillableRawImage : RawImage
    {
		public enum Type
        {
            Simple,
            Sliced,
            Tiled,
            Filled
        }

        public enum FillMethod
        {
            Horizontal,
            Vertical,
            Radial90,
            Radial180,
            Radial360,
        }

        public enum OriginHorizontal
        {
            Left,
            Right,
        }

        public enum OriginVertical
        {
            Bottom,
            Top,
        }

        public enum Origin90
        {
            BottomLeft,
            TopLeft,
            TopRight,
            BottomRight,
        }

        public enum Origin180
        {
            Bottom,
            Left,
            Top,
            Right,
        }

        public enum Origin360
        {
            Bottom,
            Right,
            Top,
            Left,
        }

		/// How the Image is drawn.
        [SerializeField] private Type m_Type = Type.Simple;
        public Type type { get { return m_Type; } set { if (SetPropertyUtility.SetStruct(ref m_Type, value)) SetVerticesDirty(); } }

        [SerializeField] private bool m_PreserveAspect = false;
        public bool preserveAspect { get { return m_PreserveAspect; } set { if (SetPropertyUtility.SetStruct(ref m_PreserveAspect, value)) SetVerticesDirty(); } }

        [SerializeField] private bool m_FillCenter = true;
        public bool fillCenter { get { return m_FillCenter; } set { if (SetPropertyUtility.SetStruct(ref m_FillCenter, value)) SetVerticesDirty(); } }

        /// Filling method for filled sprites.
        [SerializeField] private FillMethod m_FillMethod = FillMethod.Radial360;
        public FillMethod fillMethod { get { return m_FillMethod; } set { if (SetPropertyUtility.SetStruct(ref m_FillMethod, value)) {SetVerticesDirty(); m_FillOrigin = 0; } } }

        /// Amount of the Image shown. 0-1 range with 0 being nothing shown, and 1 being the full Image.
        [Range(0, 1)]
        [SerializeField] private float m_FillAmount = 1.0f;
        public float fillAmount { get { return m_FillAmount; } set { if (SetPropertyUtility.SetStruct(ref m_FillAmount, Mathf.Clamp01(value))) SetVerticesDirty(); } }

        /// Whether the Image should be filled clockwise (true) or counter-clockwise (false).
        [SerializeField] private bool m_FillClockwise = true;
        public bool fillClockwise { get { return m_FillClockwise; } set { if (SetPropertyUtility.SetStruct(ref m_FillClockwise, value)) SetVerticesDirty(); } }

        /// Controls the origin point of the Fill process. Value means different things with each fill method.
        [SerializeField] private int m_FillOrigin;
        public int fillOrigin { get { return m_FillOrigin; } set { if (SetPropertyUtility.SetStruct(ref m_FillOrigin, value)) SetVerticesDirty(); } }





		protected override void OnPopulateMesh(VertexHelper vh)
		{
			switch (type)
			{
			case Type.Simple:
				GenerateSimpleSprite(vh, m_PreserveAspect);
				break;
			case Type.Sliced:
				GenerateSlicedSprite(vh);
				break;
			case Type.Tiled:
				GenerateTiledSprite(vh);
				break;
			case Type.Filled:
				GenerateFilledSprite(vh, m_PreserveAspect);
				break;
			}
		}

		#region Various fill functions
		/// <summary>
		/// Generate vertices for a simple Image.
		/// </summary>
		void GenerateSimpleSprite(VertexHelper vh, bool lPreserveAspect)
		{
			Texture tex = mainTexture;
            vh.Clear();
            if (tex != null)
            {
                var r = GetPixelAdjustedRect();
                var v = new Vector4(r.x, r.y, r.x + r.width, r.y + r.height);

                {
                    var color32 = color;

					var uv = this.uvRect;

					vh.AddVert(new Vector3(v.x, v.y), color32, new Vector2(uv.xMin, uv.yMin));
					vh.AddVert(new Vector3(v.x, v.w), color32, new Vector2(uv.xMin, uv.yMax));
					vh.AddVert(new Vector3(v.z, v.w), color32, new Vector2(uv.xMax, uv.yMax));
					vh.AddVert(new Vector3(v.z, v.y), color32, new Vector2(uv.xMax, uv.yMin));

                    vh.AddTriangle(0, 1, 2);
                    vh.AddTriangle(2, 3, 0);
                }
            }
		}

		/// <summary>
		/// Generate vertices for a 9-sliced Image.
		/// </summary>

		#pragma warning disable 0414
		static readonly Vector2[] s_VertScratch = new Vector2[4];
		static readonly Vector2[] s_UVScratch = new Vector2[4];
		#pragma warning restore 0414

		private void GenerateSlicedSprite(VertexHelper toFill)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Generate vertices for a tiled Image.
		/// </summary>

		void GenerateTiledSprite(VertexHelper toFill)
		{
			throw new NotImplementedException();
		}

		static void AddQuad(VertexHelper vertexHelper, Vector3[] quadPositions, Color32 color, Vector3[] quadUVs)
		{
			int startIndex = vertexHelper.currentVertCount;

			for (int i = 0; i < 4; ++i)
				vertexHelper.AddVert(quadPositions[i], color, quadUVs[i]);

			vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
			vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
		}

		static void AddQuad(VertexHelper vertexHelper, Vector2 posMin, Vector2 posMax, Color32 color, Vector2 uvMin, Vector2 uvMax)
		{
			int startIndex = vertexHelper.currentVertCount;
			vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0), color, new Vector2(uvMin.x, uvMin.y));
			vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0), color, new Vector2(uvMin.x, uvMax.y));
			vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0), color, new Vector2(uvMax.x, uvMax.y));
			vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0), color, new Vector2(uvMax.x, uvMin.y));
			vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
			vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
		}

		Vector4 GetAdjustedBorders(Vector4 border, Rect rect)
		{
			for (int axis = 0; axis <= 1; axis++)
			{
				// If the rect is smaller than the combined borders, then there's not room for the borders at their normal size.
				// In order to avoid artefacts with overlapping borders, we scale the borders down to fit.
				float combinedBorders = border[axis] + border[axis + 2];
				if (rect.size[axis] < combinedBorders && combinedBorders != 0)
				{
					float borderScaleRatio = rect.size[axis] / combinedBorders;
					border[axis] *= borderScaleRatio;
					border[axis + 2] *= borderScaleRatio;
				}
			}
			return border;
		}

		/// <summary>
		/// Generate vertices for a filled Image.
		/// </summary>

		static readonly Vector3[] s_Xy = new Vector3[4];
		static readonly Vector3[] s_Uv = new Vector3[4];
		void GenerateFilledSprite(VertexHelper toFill, bool preserveAspect)
		{
			toFill.Clear();

			if (m_FillAmount < 0.001f)
				return;

			//LARRY CHANGED
			var r = GetPixelAdjustedRect();
			var v = new Vector4(r.x, r.y, r.x + r.width, r.y + r.height);

			var uv = this.uvRect;

			var outer = new Vector4(uv.xMin, uv.yMin, uv.xMax, uv.yMax);



			UIVertex uiv = UIVertex.simpleVert;
			uiv.color = color;

			float tx0 = outer.x;
			float ty0 = outer.y;
			float tx1 = outer.z;
			float ty1 = outer.w;

			// Horizontal and vertical filled sprites are simple -- just end the Image prematurely
			if (m_FillMethod == FillMethod.Horizontal || m_FillMethod == FillMethod.Vertical)
			{
				if (fillMethod == FillMethod.Horizontal)
				{
					float fill = (tx1 - tx0) * m_FillAmount;

					if (m_FillOrigin == 1)
					{
						v.x = v.z - (v.z - v.x) * m_FillAmount;
						tx0 = tx1 - fill;
					}
					else
					{
						v.z = v.x + (v.z - v.x) * m_FillAmount;
						tx1 = tx0 + fill;
					}
				}
				else if (fillMethod == FillMethod.Vertical)
				{
					float fill = (ty1 - ty0) * m_FillAmount;

					if (m_FillOrigin == 1)
					{
						v.y = v.w - (v.w - v.y) * m_FillAmount;
						ty0 = ty1 - fill;
					}
					else
					{
						v.w = v.y + (v.w - v.y) * m_FillAmount;
						ty1 = ty0 + fill;
					}
				}
			}

			s_Xy[0] = new Vector2(v.x, v.y);
			s_Xy[1] = new Vector2(v.x, v.w);
			s_Xy[2] = new Vector2(v.z, v.w);
			s_Xy[3] = new Vector2(v.z, v.y);

			s_Uv[0] = new Vector2(tx0, ty0);
			s_Uv[1] = new Vector2(tx0, ty1);
			s_Uv[2] = new Vector2(tx1, ty1);
			s_Uv[3] = new Vector2(tx1, ty0);

			{
				if (m_FillAmount < 1f && m_FillMethod != FillMethod.Horizontal && m_FillMethod != FillMethod.Vertical)
				{
					if (fillMethod == FillMethod.Radial90)
					{
						if (RadialCut(s_Xy, s_Uv, m_FillAmount, m_FillClockwise, m_FillOrigin))
							AddQuad(toFill, s_Xy, color, s_Uv);
					}
					else if (fillMethod == FillMethod.Radial180)
					{
						for (int side = 0; side < 2; ++side)
						{
							float fx0, fx1, fy0, fy1;
							int even = m_FillOrigin > 1 ? 1 : 0;

							if (m_FillOrigin == 0 || m_FillOrigin == 2)
							{
								fy0 = 0f;
								fy1 = 1f;
								if (side == even)
								{
									fx0 = 0f;
									fx1 = 0.5f;
								}
								else
								{
									fx0 = 0.5f;
									fx1 = 1f;
								}
							}
							else
							{
								fx0 = 0f;
								fx1 = 1f;
								if (side == even)
								{
									fy0 = 0.5f;
									fy1 = 1f;
								}
								else
								{
									fy0 = 0f;
									fy1 = 0.5f;
								}
							}

							s_Xy[0].x = Mathf.Lerp(v.x, v.z, fx0);
							s_Xy[1].x = s_Xy[0].x;
							s_Xy[2].x = Mathf.Lerp(v.x, v.z, fx1);
							s_Xy[3].x = s_Xy[2].x;

							s_Xy[0].y = Mathf.Lerp(v.y, v.w, fy0);
							s_Xy[1].y = Mathf.Lerp(v.y, v.w, fy1);
							s_Xy[2].y = s_Xy[1].y;
							s_Xy[3].y = s_Xy[0].y;

							s_Uv[0].x = Mathf.Lerp(tx0, tx1, fx0);
							s_Uv[1].x = s_Uv[0].x;
							s_Uv[2].x = Mathf.Lerp(tx0, tx1, fx1);
							s_Uv[3].x = s_Uv[2].x;

							s_Uv[0].y = Mathf.Lerp(ty0, ty1, fy0);
							s_Uv[1].y = Mathf.Lerp(ty0, ty1, fy1);
							s_Uv[2].y = s_Uv[1].y;
							s_Uv[3].y = s_Uv[0].y;

							float val = m_FillClockwise ? fillAmount * 2f - side : m_FillAmount * 2f - (1 - side);

							if (RadialCut(s_Xy, s_Uv, Mathf.Clamp01(val), m_FillClockwise, ((side + m_FillOrigin + 3) % 4)))
							{
								AddQuad(toFill, s_Xy, color, s_Uv);
							}
						}
					}
					else if (fillMethod == FillMethod.Radial360)
					{
						for (int corner = 0; corner < 4; ++corner)
						{
							float fx0, fx1, fy0, fy1;

							if (corner < 2)
							{
								fx0 = 0f;
								fx1 = 0.5f;
							}
							else
							{
								fx0 = 0.5f;
								fx1 = 1f;
							}

							if (corner == 0 || corner == 3)
							{
								fy0 = 0f;
								fy1 = 0.5f;
							}
							else
							{
								fy0 = 0.5f;
								fy1 = 1f;
							}

							s_Xy[0].x = Mathf.Lerp(v.x, v.z, fx0);
							s_Xy[1].x = s_Xy[0].x;
							s_Xy[2].x = Mathf.Lerp(v.x, v.z, fx1);
							s_Xy[3].x = s_Xy[2].x;

							s_Xy[0].y = Mathf.Lerp(v.y, v.w, fy0);
							s_Xy[1].y = Mathf.Lerp(v.y, v.w, fy1);
							s_Xy[2].y = s_Xy[1].y;
							s_Xy[3].y = s_Xy[0].y;

							s_Uv[0].x = Mathf.Lerp(tx0, tx1, fx0);
							s_Uv[1].x = s_Uv[0].x;
							s_Uv[2].x = Mathf.Lerp(tx0, tx1, fx1);
							s_Uv[3].x = s_Uv[2].x;

							s_Uv[0].y = Mathf.Lerp(ty0, ty1, fy0);
							s_Uv[1].y = Mathf.Lerp(ty0, ty1, fy1);
							s_Uv[2].y = s_Uv[1].y;
							s_Uv[3].y = s_Uv[0].y;

							float val = m_FillClockwise ?
								m_FillAmount * 4f - ((corner + m_FillOrigin) % 4) :
								m_FillAmount * 4f - (3 - ((corner + m_FillOrigin) % 4));

							if (RadialCut(s_Xy, s_Uv, Mathf.Clamp01(val), m_FillClockwise, ((corner + 2) % 4)))
								AddQuad(toFill, s_Xy, color, s_Uv);
						}
					}
				}
				else
				{
					AddQuad(toFill, s_Xy, color, s_Uv);
				}
			}
		}

		/// <summary>
		/// Adjust the specified quad, making it be radially filled instead.
		/// </summary>

		static bool RadialCut(Vector3[] xy, Vector3[] uv, float fill, bool invert, int corner)
		{
			// Nothing to fill
			if (fill < 0.001f) return false;

			// Even corners invert the fill direction
			if ((corner & 1) == 1) invert = !invert;

			// Nothing to adjust
			if (!invert && fill > 0.999f) return true;

			// Convert 0-1 value into 0 to 90 degrees angle in radians
			float angle = Mathf.Clamp01(fill);
			if (invert) angle = 1f - angle;
			angle *= 90f * Mathf.Deg2Rad;

			// Calculate the effective X and Y factors
			float cos = Mathf.Cos(angle);
			float sin = Mathf.Sin(angle);

			RadialCut(xy, cos, sin, invert, corner);
			RadialCut(uv, cos, sin, invert, corner);
			return true;
		}

		/// <summary>
		/// Adjust the specified quad, making it be radially filled instead.
		/// </summary>

		static void RadialCut(Vector3[] xy, float cos, float sin, bool invert, int corner)
		{
			int i0 = corner;
			int i1 = ((corner + 1) % 4);
			int i2 = ((corner + 2) % 4);
			int i3 = ((corner + 3) % 4);

			if ((corner & 1) == 1)
			{
				if (sin > cos)
				{
					cos /= sin;
					sin = 1f;

					if (invert)
					{
						xy[i1].x = Mathf.Lerp(xy[i0].x, xy[i2].x, cos);
						xy[i2].x = xy[i1].x;
					}
				}
				else if (cos > sin)
				{
					sin /= cos;
					cos = 1f;

					if (!invert)
					{
						xy[i2].y = Mathf.Lerp(xy[i0].y, xy[i2].y, sin);
						xy[i3].y = xy[i2].y;
					}
				}
				else
				{
					cos = 1f;
					sin = 1f;
				}

				if (!invert) xy[i3].x = Mathf.Lerp(xy[i0].x, xy[i2].x, cos);
				else xy[i1].y = Mathf.Lerp(xy[i0].y, xy[i2].y, sin);
			}
			else
			{
				if (cos > sin)
				{
					sin /= cos;
					cos = 1f;

					if (!invert)
					{
						xy[i1].y = Mathf.Lerp(xy[i0].y, xy[i2].y, sin);
						xy[i2].y = xy[i1].y;
					}
				}
				else if (sin > cos)
				{
					cos /= sin;
					sin = 1f;

					if (invert)
					{
						xy[i2].x = Mathf.Lerp(xy[i0].x, xy[i2].x, cos);
						xy[i3].x = xy[i2].x;
					}
				}
				else
				{
					cos = 1f;
					sin = 1f;
				}

				if (invert) xy[i3].y = Mathf.Lerp(xy[i0].y, xy[i2].y, sin);
				else xy[i1].x = Mathf.Lerp(xy[i0].x, xy[i2].x, cos);
			}
		}

		#endregion

		public virtual void CalculateLayoutInputHorizontal() {}
		public virtual void CalculateLayoutInputVertical() {}

		public virtual float minWidth { get { return 0; } }

		public virtual float preferredWidth
		{
			get
			{
				return 0;
			}
		}

		public virtual float flexibleWidth { get { return -1; } }

		public virtual float minHeight { get { return 0; } }

		public virtual float preferredHeight
		{
			get
			{
				return 0;
			}
		}

		public virtual float flexibleHeight { get { return -1; } }

		public virtual int layoutPriority { get { return 0; } }

		public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
		{
			return true;
		}

    }
}

