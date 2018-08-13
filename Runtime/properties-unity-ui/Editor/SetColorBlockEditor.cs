using BeatThat.Properties;
using UnityEditor;
using UnityEngine;

namespace BeatThat.Properties.UnityUI
{
	[CustomEditor(typeof(SetColorBlock), true)]
	[CanEditMultipleObjects]
	public class SetColorBlockEditor : UnityEditor.Editor
	{
		override public void OnInspectorGUI() 
		{
			EditorGUI.BeginChangeCheck();

			var hasColorProp = this.serializedObject.FindProperty("m_hasColorBlock");
			EditorGUILayout.PropertyField(hasColorProp, new GUIContent("Target", "HasColorBlock component that to be updated"));

			var userAssetProp = this.serializedObject.FindProperty("m_useAsset");

			if(userAssetProp.boolValue) {
				var assetProp = this.serializedObject.FindProperty("m_colorBlockAsset");
				EditorGUILayout.PropertyField(assetProp, new GUIContent("ColorBlock", "a shared ColorBlockAsset for places where you want consistency"));
			}
			else {
				var colorProp = this.serializedObject.FindProperty("m_colorBlock");
				EditorGUILayout.PropertyField(colorProp);
			}

			EditorGUILayout.PropertyField(userAssetProp, new GUIContent("Use ColorBlock Asset", "Use a shared color asset or a local color property?"));

			this.serializedObject.ApplyModifiedProperties();

			if(EditorGUI.EndChangeCheck()) {
				(this.target as SetColorBlock).UpdateDisplay();
			}


		}
	}
}
