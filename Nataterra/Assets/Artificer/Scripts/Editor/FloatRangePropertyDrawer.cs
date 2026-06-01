using UnityEditor;
using UnityEngine;

namespace Artifice
{
	[CustomPropertyDrawer(typeof(FloatRange))]
	public class FloatRangeDrawer : PropertyDrawer
	{
		static readonly string[] modeLabels = { "Constant", "Random Between Two Values" };

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var modeProp = property.FindPropertyRelative("mode");
			//var valueProp = property.FindPropertyRelative("value");
			var minProp = property.FindPropertyRelative("min");
			var maxProp = property.FindPropertyRelative("max");

			// Draw label and get remaining rect
			Rect contentRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			const float PopupWidth = 18f;

			// Popup on the right
			Rect popupRect = new Rect(contentRect.xMax - PopupWidth, contentRect.y, PopupWidth, contentRect.height);

			// Field area (everything left of popup)
			Rect fieldRect = new Rect(contentRect.x, contentRect.y, contentRect.width - PopupWidth - 2, contentRect.height);

			if ( (FloatRange.Mode)modeProp.enumValueIndex == FloatRange.Mode.Constant )
			{
				float oldLabelWidth = EditorGUIUtility.labelWidth;
				EditorGUIUtility.labelWidth = 12f;
				EditorGUI.PropertyField(fieldRect, maxProp, new GUIContent("V"));
				EditorGUIUtility.labelWidth = oldLabelWidth;
			}
			else
			{
				float half = fieldRect.width * 0.5f;

				Rect minRect = new Rect(fieldRect.x, fieldRect.y, half - 2, fieldRect.height);
				Rect maxRect = new Rect(fieldRect.x + half + 2, fieldRect.y, half - 2, fieldRect.height);

				// Shrink label so numeric dragging works nicely
				float oldLabelWidth = EditorGUIUtility.labelWidth;
				EditorGUIUtility.labelWidth = 24f;

				EditorGUI.PropertyField(minRect, minProp, new GUIContent("Min"));
				EditorGUI.PropertyField(maxRect, maxProp, new GUIContent("Max"));

				EditorGUIUtility.labelWidth = oldLabelWidth;
			}

			modeProp.enumValueIndex =
				EditorGUI.Popup(popupRect, modeProp.enumValueIndex, modeLabels);

			EditorGUI.EndProperty();
		}
	}
}