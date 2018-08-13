using System.Linq;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    /// <summary>
    /// Editor class used to edit UI Images.
    /// </summary>
    [CustomEditor(typeof(FillableRawImage), true)]
	[CanEditMultipleObjects]
	public class FillableRawImageEditor : GraphicEditor
	{
		SerializedProperty m_Texture;
		SerializedProperty m_UVRect;
		GUIContent m_UVRectContent;

		protected override void OnEnable()
		{
			base.OnEnable();

			// Note we have precedence for calling rectangle for just rect, even in the Inspector.
			// For example in the Camera component's Viewport Rect.
			// Hence sticking with Rect here to be consistent with corresponding property in the API.
			m_UVRectContent     = new GUIContent("UV Rect");

			m_Texture           = serializedObject.FindProperty("m_Texture");
			m_UVRect            = serializedObject.FindProperty("m_UVRect");



		
			//			m_SpriteContent = new GUIContent("Source Image");
			m_SpriteTypeContent     = new GUIContent("Image Type");
			m_ClockwiseContent      = new GUIContent("Clockwise");

			//			m_Sprite                = serializedObject.FindProperty("m_Sprite");
			m_Type                  = serializedObject.FindProperty("m_Type");
			m_FillCenter            = serializedObject.FindProperty("m_FillCenter");
			m_FillMethod            = serializedObject.FindProperty("m_FillMethod");
			m_FillOrigin            = serializedObject.FindProperty("m_FillOrigin");
			m_FillClockwise         = serializedObject.FindProperty("m_FillClockwise");
			m_FillAmount            = serializedObject.FindProperty("m_FillAmount");
			m_PreserveAspect        = serializedObject.FindProperty("m_PreserveAspect");

			//			m_ShowType = new AnimBool(m_Sprite.objectReferenceValue != null);
			m_ShowType = new AnimBool(true); //m_Texture.objectReferenceValue != null);

			m_ShowType.valueChanged.AddListener(Repaint);

			var typeEnum = (FillableRawImage.Type)m_Type.enumValueIndex;

			m_ShowSlicedOrTiled = new AnimBool(!m_Type.hasMultipleDifferentValues && typeEnum == FillableRawImage.Type.Sliced);
			m_ShowSliced = new AnimBool(!m_Type.hasMultipleDifferentValues && typeEnum == FillableRawImage.Type.Sliced);
			m_ShowFilled = new AnimBool(!m_Type.hasMultipleDifferentValues && typeEnum == FillableRawImage.Type.Filled);
			m_ShowSlicedOrTiled.valueChanged.AddListener(Repaint);
			m_ShowSliced.valueChanged.AddListener(Repaint);
			m_ShowFilled.valueChanged.AddListener(Repaint);




			SetShowNativeSize(true);
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(m_Texture);
			AppearanceControlsGUI();
			RaycastControlsGUI();




			EditorGUILayout.PropertyField(m_UVRect, m_UVRectContent);
//			SetShowNativeSize(false);



			m_ShowType.target = true; //m_Texture.objectReferenceValue != null;


			if (EditorGUILayout.BeginFadeGroup(m_ShowType.faded))
				TypeGUI();
			EditorGUILayout.EndFadeGroup();

			SetShowNativeSize(false);
			if (EditorGUILayout.BeginFadeGroup(m_ShowNativeSize.faded))
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(m_PreserveAspect);
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFadeGroup();






			NativeSizeButtonGUI();

			serializedObject.ApplyModifiedProperties();
		}

		void SetShowNativeSize(bool instant)
		{
			if(m_Texture.objectReferenceValue == null) {
				base.SetShowNativeSize(false, instant);
				return;
			}

			FillableRawImage.Type type = (FillableRawImage.Type)m_Type.enumValueIndex;
			bool showNativeSize = (type == FillableRawImage.Type.Simple || type == FillableRawImage.Type.Filled);
			base.SetShowNativeSize(showNativeSize, instant);
		}

		/// <summary>
		/// Allow the texture to be previewed.
		/// </summary>

		public override bool HasPreviewGUI()
		{
			FillableRawImage rawImage = target as FillableRawImage;
			return rawImage != null;
		}

		/// <summary>
		/// Draw the Image preview.
		/// </summary>

		public override void OnPreviewGUI(Rect rect, GUIStyle background)
		{
			FillableRawImage rawImage = target as FillableRawImage;
			Texture tex = rawImage.mainTexture;

			if (tex == null)
				return;

			Rect outer = rawImage.uvRect;
			outer.xMin *= rawImage.rectTransform.rect.width;
			outer.xMax *= rawImage.rectTransform.rect.width;
			outer.yMin *= rawImage.rectTransform.rect.height;
			outer.yMax *= rawImage.rectTransform.rect.height;

			SpriteDrawUtility.DrawSprite(tex, rect, outer, rawImage.uvRect, rawImage.canvasRenderer.GetColor());
		}

		/// <summary>
		/// Info String drawn at the bottom of the Preview
		/// </summary>

		public override string GetInfoString()
		{
			FillableRawImage rawImage = target as FillableRawImage;

			// Image size Text
			string text = string.Format("FillableRawImage Size: {0}x{1}",
				Mathf.RoundToInt(Mathf.Abs(rawImage.rectTransform.rect.width)),
				Mathf.RoundToInt(Mathf.Abs(rawImage.rectTransform.rect.height)));

			return text;
		}
//	}

//	public class ImageEditor : GraphicEditor
//	{
		SerializedProperty m_FillMethod;
		SerializedProperty m_FillOrigin;
		SerializedProperty m_FillAmount;
		SerializedProperty m_FillClockwise;
		SerializedProperty m_Type;
		SerializedProperty m_FillCenter;
//		SerializedProperty m_Sprite;
		SerializedProperty m_PreserveAspect;
//		GUIContent m_SpriteContent;
		GUIContent m_SpriteTypeContent;
		GUIContent m_ClockwiseContent;
		AnimBool m_ShowSlicedOrTiled;
		AnimBool m_ShowSliced;
		AnimBool m_ShowFilled;
		AnimBool m_ShowType;

//		protected override void OnEnable()
//		{
//			base.OnEnable();
//
////			m_SpriteContent = new GUIContent("Source Image");
//			m_SpriteTypeContent     = new GUIContent("Image Type");
//			m_ClockwiseContent      = new GUIContent("Clockwise");
//
////			m_Sprite                = serializedObject.FindProperty("m_Sprite");
//			m_Type                  = serializedObject.FindProperty("m_Type");
//			m_FillCenter            = serializedObject.FindProperty("m_FillCenter");
//			m_FillMethod            = serializedObject.FindProperty("m_FillMethod");
//			m_FillOrigin            = serializedObject.FindProperty("m_FillOrigin");
//			m_FillClockwise         = serializedObject.FindProperty("m_FillClockwise");
//			m_FillAmount            = serializedObject.FindProperty("m_FillAmount");
//			m_PreserveAspect        = serializedObject.FindProperty("m_PreserveAspect");
//
////			m_ShowType = new AnimBool(m_Sprite.objectReferenceValue != null);
//			m_ShowType = new AnimBool(m_Texture.objectReferenceValue != null);
//
//			m_ShowType.valueChanged.AddListener(Repaint);
//
//			var typeEnum = (FillableRawImage.Type)m_Type.enumValueIndex;
//
//			m_ShowSlicedOrTiled = new AnimBool(!m_Type.hasMultipleDifferentValues && typeEnum == FillableRawImage.Type.Sliced);
//			m_ShowSliced = new AnimBool(!m_Type.hasMultipleDifferentValues && typeEnum == FillableRawImage.Type.Sliced);
//			m_ShowFilled = new AnimBool(!m_Type.hasMultipleDifferentValues && typeEnum == FillableRawImage.Type.Filled);
//			m_ShowSlicedOrTiled.valueChanged.AddListener(Repaint);
//			m_ShowSliced.valueChanged.AddListener(Repaint);
//			m_ShowFilled.valueChanged.AddListener(Repaint);
//
//			SetShowNativeSize(true);
//		}

		protected override void OnDisable()
		{
			m_ShowType.valueChanged.RemoveListener(Repaint);
			m_ShowSlicedOrTiled.valueChanged.RemoveListener(Repaint);
			m_ShowSliced.valueChanged.RemoveListener(Repaint);
			m_ShowFilled.valueChanged.RemoveListener(Repaint);
		}

//		public override void OnInspectorGUI()
//		{
//			serializedObject.Update();
//
//			SpriteGUI();
//			AppearanceControlsGUI();
//			RaycastControlsGUI();
//
////			m_ShowType.target = m_Sprite.objectReferenceValue != null;
//			m_ShowType.target = m_Texture.objectReferenceValue != null;
//
//
//			if (EditorGUILayout.BeginFadeGroup(m_ShowType.faded))
//				TypeGUI();
//			EditorGUILayout.EndFadeGroup();
//
//			SetShowNativeSize(false);
//			if (EditorGUILayout.BeginFadeGroup(m_ShowNativeSize.faded))
//			{
//				EditorGUI.indentLevel++;
//				EditorGUILayout.PropertyField(m_PreserveAspect);
//				EditorGUI.indentLevel--;
//			}
//			EditorGUILayout.EndFadeGroup();
//			NativeSizeButtonGUI();
//
//			serializedObject.ApplyModifiedProperties();
//		}

//		void SetShowNativeSize(bool instant)
//		{
//			FillableRawImage.Type type = (FillableRawImage.Type)m_Type.enumValueIndex;
//			bool showNativeSize = (type == FillableRawImage.Type.Simple || type == FillableRawImage.Type.Filled);
//			base.SetShowNativeSize(showNativeSize, instant);
//		}

		/// <summary>
		/// Draw the atlas and Image selection fields.
		/// </summary>

//		protected void SpriteGUI()
//		{
//			EditorGUI.BeginChangeCheck();
//
////			EditorGUILayout.PropertyField(m_Sprite, m_SpriteContent);
//
//			if (EditorGUI.EndChangeCheck())
//			{
//				var newSprite = m_Sprite.objectReferenceValue as Sprite;
//				if (newSprite)
//				{
//					FillableRawImage.Type oldType = (FillableRawImage.Type)m_Type.enumValueIndex;
//					if (newSprite.border.SqrMagnitude() > 0)
//					{
//						m_Type.enumValueIndex = (int)FillableRawImage.Type.Sliced;
//					}
//					else if (oldType == FillableRawImage.Type.Sliced)
//					{
//						m_Type.enumValueIndex = (int)FillableRawImage.Type.Simple;
//					}
//				}
//			}
//		}

		/// <summary>
		/// Sprites's custom properties based on the type.
		/// </summary>

		protected void TypeGUI()
		{
			EditorGUILayout.PropertyField(m_Type, m_SpriteTypeContent);

			++EditorGUI.indentLevel;
			{
				FillableRawImage.Type typeEnum = (FillableRawImage.Type)m_Type.enumValueIndex;

				bool showSlicedOrTiled = (!m_Type.hasMultipleDifferentValues && (typeEnum == FillableRawImage.Type.Sliced || typeEnum == FillableRawImage.Type.Tiled));
				if (showSlicedOrTiled && targets.Length > 1)
					showSlicedOrTiled = targets.Select(obj => obj as Image).All(img => img.hasBorder);

				m_ShowSlicedOrTiled.target = showSlicedOrTiled;
				m_ShowSliced.target = (showSlicedOrTiled && !m_Type.hasMultipleDifferentValues && typeEnum == FillableRawImage.Type.Sliced);
				m_ShowFilled.target = (!m_Type.hasMultipleDifferentValues && typeEnum == FillableRawImage.Type.Filled);

				Image image = target as Image;
				if (EditorGUILayout.BeginFadeGroup(m_ShowSlicedOrTiled.faded))
				{
					if (image.hasBorder)
						EditorGUILayout.PropertyField(m_FillCenter);
				}
				EditorGUILayout.EndFadeGroup();

				if (EditorGUILayout.BeginFadeGroup(m_ShowSliced.faded))
				{
					if (image.sprite != null && !image.hasBorder)
						EditorGUILayout.HelpBox("This Image doesn't have a border.", MessageType.Warning);
				}
				EditorGUILayout.EndFadeGroup();

				if (EditorGUILayout.BeginFadeGroup(m_ShowFilled.faded))
				{
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(m_FillMethod);
					if (EditorGUI.EndChangeCheck())
					{
						m_FillOrigin.intValue = 0;
					}
					switch ((Image.FillMethod)m_FillMethod.enumValueIndex)
					{
					case Image.FillMethod.Horizontal:
						m_FillOrigin.intValue = (int)(Image.OriginHorizontal)EditorGUILayout.EnumPopup("Fill Origin", (Image.OriginHorizontal)m_FillOrigin.intValue);
						break;
					case Image.FillMethod.Vertical:
						m_FillOrigin.intValue = (int)(Image.OriginVertical)EditorGUILayout.EnumPopup("Fill Origin", (Image.OriginVertical)m_FillOrigin.intValue);
						break;
					case Image.FillMethod.Radial90:
						m_FillOrigin.intValue = (int)(Image.Origin90)EditorGUILayout.EnumPopup("Fill Origin", (Image.Origin90)m_FillOrigin.intValue);
						break;
					case Image.FillMethod.Radial180:
						m_FillOrigin.intValue = (int)(Image.Origin180)EditorGUILayout.EnumPopup("Fill Origin", (Image.Origin180)m_FillOrigin.intValue);
						break;
					case Image.FillMethod.Radial360:
						m_FillOrigin.intValue = (int)(Image.Origin360)EditorGUILayout.EnumPopup("Fill Origin", (Image.Origin360)m_FillOrigin.intValue);
						break;
					}
					EditorGUILayout.PropertyField(m_FillAmount);
					if ((Image.FillMethod)m_FillMethod.enumValueIndex > Image.FillMethod.Vertical)
					{
						EditorGUILayout.PropertyField(m_FillClockwise, m_ClockwiseContent);
					}
				}
				EditorGUILayout.EndFadeGroup();
			}
			--EditorGUI.indentLevel;
		}

		/// <summary>
		/// All graphics have a preview.
		/// </summary>

//		public override bool HasPreviewGUI() { return true; }

		/// <summary>
		/// Draw the Image preview.
		/// </summary>

//		public override void OnPreviewGUI(Rect rect, GUIStyle background)
//		{
//			Image image = target as Image;
//			if (image == null) return;
//
//			Sprite sf = image.sprite;
//			if (sf == null) return;
//
//			SpriteDrawUtility.DrawSprite(sf, rect, image.canvasRenderer.GetColor());
//		}

		/// <summary>
		/// Info String drawn at the bottom of the Preview
		/// </summary>

//		public override string GetInfoString()
//		{
//			Image image = target as Image;
//			Sprite sprite = image.sprite;
//
//			int x = (sprite != null) ? Mathf.RoundToInt(sprite.rect.width) : 0;
//			int y = (sprite != null) ? Mathf.RoundToInt(sprite.rect.height) : 0;
//
//			return string.Format("Image Size: {0}x{1}", x, y);
//		}
	}
}


