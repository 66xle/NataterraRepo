using UnityEngine;
using UnityEditor;

namespace Artifice
{
	[CustomPropertyDrawer(typeof(Vector3Range))]
	public class Vector3RangeDrawer : PropertyDrawer
	{
		static readonly string[] modeLabels = { "Constant", "Random Between Two Values" };

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var modeProp = property.FindPropertyRelative("mode");
			return (Vector3Range.Mode)modeProp.enumValueIndex == Vector3Range.Mode.Constant
				? EditorGUIUtility.singleLineHeight
				: EditorGUIUtility.singleLineHeight * 2f; // two lines for min/max
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var modeProp = property.FindPropertyRelative("mode");
			var minProp = property.FindPropertyRelative("min");
			var maxProp = property.FindPropertyRelative("max");

			position = EditorGUI.PrefixLabel(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), label);

			Rect popupRect = new Rect(position.xMax - 18, position.y, 18, EditorGUIUtility.singleLineHeight);
			modeProp.enumValueIndex = EditorGUI.Popup(popupRect, modeProp.enumValueIndex, new string[] { "Constant", "Random Between Two" });

			float oldLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 12f;

			if ( (Vector3Range.Mode)modeProp.enumValueIndex == Vector3Range.Mode.Constant )
			{
				EditorGUI.PropertyField(new Rect(position.x, position.y, position.width - 20, EditorGUIUtility.singleLineHeight), maxProp, GUIContent.none);
			}
			else
			{
				// Min field on first line
				EditorGUI.PropertyField(new Rect(position.x, position.y, position.width - 20, EditorGUIUtility.singleLineHeight), minProp, GUIContent.none);	//new GUIContent(" "));
				// Max field on second line
				EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width - 20, EditorGUIUtility.singleLineHeight), maxProp, GUIContent.none);	//new GUIContent(" "));
			}

			EditorGUIUtility.labelWidth = oldLabelWidth;
			EditorGUI.EndProperty();
		}
	}
}