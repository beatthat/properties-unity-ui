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
//        [SerializeField] Texture m_Texture;
//        [SerializeField] Rect m_UVRect = new Rect(0f, 0f, 1f, 1f);
//
//		protected FillableRawImage()
//        {
//            useLegacyMeshGeneration = false;
//        }
//
//        /// <summary>
//        /// Returns the texture used to draw this Graphic.
//        /// </summary>
//        public override Texture mainTexture
//        {
//            get
//            {
//                if (m_Texture == null)
//                {
//                    if (material != null && material.mainTexture != null)
//                    {
//                        return material.mainTexture;
//                    }
//                    return s_WhiteTexture;
//                }
//
//                return m_Texture;
//            }
//        }
//
//        /// <summary>
//        /// Texture to be used.
//        /// </summary>
//        public Texture texture
//        {
//            get
//            {
//                return m_Texture;
//            }
//            set
//            {
//                if (m_Texture == value)
//                    return;
//
//                m_Texture = value;
//                SetVerticesDirty();
//                SetMaterialDirty();
//            }
//        }
//
//        /// <summary>
//        /// UV rectangle used by the texture.
//        /// </summary>
//        public Rect uvRect
//        {
//            get
//            {
//                return m_UVRect;
//            }
//            set
//            {
//                if (m_UVRect == value)
//                    return;
//                m_UVRect = value;
//                SetVerticesDirty();
//            }
//        }
//
//        /// <summary>
//        /// Adjust the scale of the Graphic to make it pixel-perfect.
//        /// </summary>
//
//        public override void SetNativeSize()
//        {
//            Texture tex = mainTexture;
//            if (tex != null)
//            {
//                int w = Mathf.RoundToInt(tex.width * uvRect.width);
//                int h = Mathf.RoundToInt(tex.height * uvRect.height);
//                rectTransform.anchorMax = rectTransform.anchorMin;
//                rectTransform.sizeDelta = new Vector2(w, h);
//            }
//        }

//        protected override void OnPopulateMesh(VertexHelper vh)
//        {
//            Texture tex = mainTexture;
//            vh.Clear();
//            if (tex != null)
//            {
//                var r = GetPixelAdjustedRect();
//                var v = new Vector4(r.x, r.y, r.x + r.width, r.y + r.height);
//
//                {
//                    var color32 = color;
//                    vh.AddVert(new Vector3(v.x, v.y), color32, new Vector2(m_UVRect.xMin, m_UVRect.yMin));
//                    vh.AddVert(new Vector3(v.x, v.w), color32, new Vector2(m_UVRect.xMin, m_UVRect.yMax));
//                    vh.AddVert(new Vector3(v.z, v.w), color32, new Vector2(m_UVRect.xMax, m_UVRect.yMax));
//                    vh.AddVert(new Vector3(v.z, v.y), color32, new Vector2(m_UVRect.xMax, m_UVRect.yMin));
//
//                    vh.AddTriangle(0, 1, 2);
//                    vh.AddTriangle(2, 3, 0);
//                }
//            }
//        }




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
//			if (overrideSprite == null)
//			{
//				base.OnPopulateMesh(vh);
//				return;
//			}

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
//			Vector4 v = GetDrawingDimensions(lPreserveAspect);
//			var uv = (overrideSprite != null) ? Sprites.DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;
//
//			var color32 = color;
//			vh.Clear();
//			vh.AddVert(new Vector3(v.x, v.y), color32, new Vector2(uv.x, uv.y));
//			vh.AddVert(new Vector3(v.x, v.w), color32, new Vector2(uv.x, uv.w));
//			vh.AddVert(new Vector3(v.z, v.w), color32, new Vector2(uv.z, uv.w));
//			vh.AddVert(new Vector3(v.z, v.y), color32, new Vector2(uv.z, uv.y));
//
//			vh.AddTriangle(0, 1, 2);
//			vh.AddTriangle(2, 3, 0);







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
//			if (!hasBorder)
//			{
//				GenerateSimpleSprite(toFill, false);
//				return;
//			}
//
//			Vector4 outer, inner, padding, border;
//
//			if (overrideSprite != null)
//			{
//				outer = Sprites.DataUtility.GetOuterUV(overrideSprite);
//				inner = Sprites.DataUtility.GetInnerUV(overrideSprite);
//				padding = Sprites.DataUtility.GetPadding(overrideSprite);
//				border = overrideSprite.border;
//			}
//			else
//			{
//				outer = Vector4.zero;
//				inner = Vector4.zero;
//				padding = Vector4.zero;
//				border = Vector4.zero;
//			}
//
//			Rect rect = GetPixelAdjustedRect();
//			border = GetAdjustedBorders(border / pixelsPerUnit, rect);
//			padding = padding / pixelsPerUnit;
//
//			s_VertScratch[0] = new Vector2(padding.x, padding.y);
//			s_VertScratch[3] = new Vector2(rect.width - padding.z, rect.height - padding.w);
//
//			s_VertScratch[1].x = border.x;
//			s_VertScratch[1].y = border.y;
//			s_VertScratch[2].x = rect.width - border.z;
//			s_VertScratch[2].y = rect.height - border.w;
//
//			for (int i = 0; i < 4; ++i)
//			{
//				s_VertScratch[i].x += rect.x;
//				s_VertScratch[i].y += rect.y;
//			}
//
//			s_UVScratch[0] = new Vector2(outer.x, outer.y);
//			s_UVScratch[1] = new Vector2(inner.x, inner.y);
//			s_UVScratch[2] = new Vector2(inner.z, inner.w);
//			s_UVScratch[3] = new Vector2(outer.z, outer.w);
//
//			toFill.Clear();
//
//			for (int x = 0; x < 3; ++x)
//			{
//				int x2 = x + 1;
//
//				for (int y = 0; y < 3; ++y)
//				{
//					if (!m_FillCenter && x == 1 && y == 1)
//						continue;
//
//					int y2 = y + 1;
//
//					AddQuad(toFill,
//						new Vector2(s_VertScratch[x].x, s_VertScratch[y].y),
//						new Vector2(s_VertScratch[x2].x, s_VertScratch[y2].y),
//						color,
//						new Vector2(s_UVScratch[x].x, s_UVScratch[y].y),
//						new Vector2(s_UVScratch[x2].x, s_UVScratch[y2].y));
//				}
//			}
		}

		/// <summary>
		/// Generate vertices for a tiled Image.
		/// </summary>

		void GenerateTiledSprite(VertexHelper toFill)
		{
			throw new NotImplementedException();

//			Vector4 outer, inner, border;
//			Vector2 spriteSize;
//
//			if (overrideSprite != null)
//			{
//				outer = Sprites.DataUtility.GetOuterUV(overrideSprite);
//				inner = Sprites.DataUtility.GetInnerUV(overrideSprite);
//				border = overrideSprite.border;
//				spriteSize = overrideSprite.rect.size;
//			}
//			else
//			{
//				outer = Vector4.zero;
//				inner = Vector4.zero;
//				border = Vector4.zero;
//				spriteSize = Vector2.one * 100;
//			}
//
//			Rect rect = GetPixelAdjustedRect();
//			float tileWidth = (spriteSize.x - border.x - border.z) / pixelsPerUnit;
//			float tileHeight = (spriteSize.y - border.y - border.w) / pixelsPerUnit;
//			border = GetAdjustedBorders(border / pixelsPerUnit, rect);
//
//			var uvMin = new Vector2(inner.x, inner.y);
//			var uvMax = new Vector2(inner.z, inner.w);
//
//			var v = UIVertex.simpleVert;
//			v.color = color;
//
//			// Min to max max range for tiled region in coordinates relative to lower left corner.
//			float xMin = border.x;
//			float xMax = rect.width - border.z;
//			float yMin = border.y;
//			float yMax = rect.height - border.w;
//
//			toFill.Clear();
//			var clipped = uvMax;
//
//			// if either with is zero we cant tile so just assume it was the full width.
//			if (tileWidth == 0)
//				tileWidth = xMax - xMin;
//
//			if (tileHeight == 0)
//				tileHeight = yMax - yMin;
//
//			if (m_FillCenter)
//			{
//				for (float y1 = yMin; y1 < yMax; y1 += tileHeight)
//				{
//					float y2 = y1 + tileHeight;
//					if (y2 > yMax)
//					{
//						clipped.y = uvMin.y + (uvMax.y - uvMin.y) * (yMax - y1) / (y2 - y1);
//						y2 = yMax;
//					}
//
//					clipped.x = uvMax.x;
//					for (float x1 = xMin; x1 < xMax; x1 += tileWidth)
//					{
//						float x2 = x1 + tileWidth;
//						if (x2 > xMax)
//						{
//							clipped.x = uvMin.x + (uvMax.x - uvMin.x) * (xMax - x1) / (x2 - x1);
//							x2 = xMax;
//						}
//						AddQuad(toFill, new Vector2(x1, y1) + rect.position, new Vector2(x2, y2) + rect.position, color, uvMin, clipped);
//					}
//				}
//			}
//
//			if (hasBorder)
//			{
//				clipped = uvMax;
//				for (float y1 = yMin; y1 < yMax; y1 += tileHeight)
//				{
//					float y2 = y1 + tileHeight;
//					if (y2 > yMax)
//					{
//						clipped.y = uvMin.y + (uvMax.y - uvMin.y) * (yMax - y1) / (y2 - y1);
//						y2 = yMax;
//					}
//					AddQuad(toFill,
//						new Vector2(0, y1) + rect.position,
//						new Vector2(xMin, y2) + rect.position,
//						color,
//						new Vector2(outer.x, uvMin.y),
//						new Vector2(uvMin.x, clipped.y));
//					AddQuad(toFill,
//						new Vector2(xMax, y1) + rect.position,
//						new Vector2(rect.width, y2) + rect.position,
//						color,
//						new Vector2(uvMax.x, uvMin.y),
//						new Vector2(outer.z, clipped.y));
//				}
//
//				// Bottom and top tiled border
//				clipped = uvMax;
//				for (float x1 = xMin; x1 < xMax; x1 += tileWidth)
//				{
//					float x2 = x1 + tileWidth;
//					if (x2 > xMax)
//					{
//						clipped.x = uvMin.x + (uvMax.x - uvMin.x) * (xMax - x1) / (x2 - x1);
//						x2 = xMax;
//					}
//					AddQuad(toFill,
//						new Vector2(x1, 0) + rect.position,
//						new Vector2(x2, yMin) + rect.position,
//						color,
//						new Vector2(uvMin.x, outer.y),
//						new Vector2(clipped.x, uvMin.y));
//					AddQuad(toFill,
//						new Vector2(x1, yMax) + rect.position,
//						new Vector2(x2, rect.height) + rect.position,
//						color,
//						new Vector2(uvMin.x, uvMax.y),
//						new Vector2(clipped.x, outer.w));
//				}
//
//				// Corners
//				AddQuad(toFill,
//					new Vector2(0, 0) + rect.position,
//					new Vector2(xMin, yMin) + rect.position,
//					color,
//					new Vector2(outer.x, outer.y),
//					new Vector2(uvMin.x, uvMin.y));
//				AddQuad(toFill,
//					new Vector2(xMax, 0) + rect.position,
//					new Vector2(rect.width, yMin) + rect.position,
//					color,
//					new Vector2(uvMax.x, outer.y),
//					new Vector2(outer.z, uvMin.y));
//				AddQuad(toFill,
//					new Vector2(0, yMax) + rect.position,
//					new Vector2(xMin, rect.height) + rect.position,
//					color,
//					new Vector2(outer.x, uvMax.y),
//					new Vector2(uvMin.x, outer.w));
//				AddQuad(toFill,
//					new Vector2(xMax, yMax) + rect.position,
//					new Vector2(rect.width, rect.height) + rect.position,
//					color,
//					new Vector2(uvMax.x, uvMax.y),
//					new Vector2(outer.z, outer.w));
//			}
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

//			Vector4 v = GetDrawingDimensions(preserveAspect);
//			Vector4 outer = overrideSprite != null ? Sprites.DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;




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

//				if (overrideSprite == null)
//					return 0;
//				if (type == Type.Sliced || type == Type.Tiled)
//					return Sprites.DataUtility.GetMinSize(overrideSprite).x / pixelsPerUnit;
//				return overrideSprite.rect.size.x / pixelsPerUnit;
			}
		}

		public virtual float flexibleWidth { get { return -1; } }

		public virtual float minHeight { get { return 0; } }

		public virtual float preferredHeight
		{
			get
			{
				return 0;

//				if (overrideSprite == null)
//					return 0;
//				if (type == Type.Sliced || type == Type.Tiled)
//					return Sprites.DataUtility.GetMinSize(overrideSprite).y / pixelsPerUnit;
//				return overrideSprite.rect.size.y / pixelsPerUnit;
			}
		}

		public virtual float flexibleHeight { get { return -1; } }

		public virtual int layoutPriority { get { return 0; } }

		public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
		{
			return true;

//			if (m_EventAlphaThreshold >= 1)
//				return true;
//
//			Sprite sprite = overrideSprite;
//			if (sprite == null)
//				return true;
//
//			Vector2 local;
//			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out local);
//
//			Rect rect = GetPixelAdjustedRect();
//
//			// Convert to have lower left corner as reference point.
//			local.x += rectTransform.pivot.x * rect.width;
//			local.y += rectTransform.pivot.y * rect.height;
//
//			local = MapCoordinate(local, rect);
//
//			// Normalize local coordinates.
//			Rect spriteRect = sprite.textureRect;
//			Vector2 normalized = new Vector2(local.x / spriteRect.width, local.y / spriteRect.height);
//
//			// Convert to texture space.
//			float x = Mathf.Lerp(spriteRect.x, spriteRect.xMax, normalized.x) / sprite.texture.width;
//			float y = Mathf.Lerp(spriteRect.y, spriteRect.yMax, normalized.y) / sprite.texture.height;
//
//			try
//			{
//				return sprite.texture.GetPixelBilinear(x, y).a >= m_EventAlphaThreshold;
//			}
//			catch (UnityException e)
//			{
//				Debug.LogError("Using clickAlphaThreshold lower than 1 on Image whose sprite texture cannot be read. " + e.Message + " Also make sure to disable sprite packing for this sprite.", this);
//				return true;
//			}
		}

//		private Vector2 MapCoordinate(Vector2 local, Rect rect)
//		{
//			Rect spriteRect = sprite.rect;
//			if (type == Type.Simple || type == Type.Filled)
//				return new Vector2(local.x * spriteRect.width / rect.width, local.y * spriteRect.height / rect.height);
//
//			Vector4 border = sprite.border;
//			Vector4 adjustedBorder = GetAdjustedBorders(border / pixelsPerUnit, rect);
//
//			for (int i = 0; i < 2; i++)
//			{
//				if (local[i] <= adjustedBorder[i])
//					continue;
//
//				if (rect.size[i] - local[i] <= adjustedBorder[i + 2])
//				{
//					local[i] -= (rect.size[i] - spriteRect.size[i]);
//					continue;
//				}
//
//				if (type == Type.Sliced)
//				{
//					float lerp = Mathf.InverseLerp(adjustedBorder[i], rect.size[i] - adjustedBorder[i + 2], local[i]);
//					local[i] = Mathf.Lerp(border[i], spriteRect.size[i] - border[i + 2], lerp);
//					continue;
//				}
//				else
//				{
//					local[i] -= adjustedBorder[i];
//					local[i] = Mathf.Repeat(local[i], spriteRect.size[i] - border[i] - border[i + 2]);
//					local[i] += border[i];
//					continue;
//				}
//			}
//
//			return local;
//		}
    }
}

