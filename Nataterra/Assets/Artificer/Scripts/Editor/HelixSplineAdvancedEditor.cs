#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

namespace Artifice
{
	[CustomEditor(typeof(HelixSplineAdvanced))]
	[CanEditMultipleObjects]
	public class HelixSplineAdvancedEditor : Editor
	{
		SerializedProperty radius1Prop, radius2Prop, heightProp, turnsProp, biasProp, clockwiseProp, pointsPerTurnProp, loopProp;
		SerializedProperty upModeProp, axisProp, extraTwistProp, useTurnTwistProp;
		SerializedProperty loopBlendProp, loopForceProp, generateOnValidateProp, reverse;

		void OnEnable()
		{
			radius1Prop				= serializedObject.FindProperty("radius1");
			radius2Prop				= serializedObject.FindProperty("radius2");
			heightProp				= serializedObject.FindProperty("height");
			turnsProp				= serializedObject.FindProperty("turns");
			biasProp				= serializedObject.FindProperty("bias");
			clockwiseProp			= serializedObject.FindProperty("clockwise");
			pointsPerTurnProp		= serializedObject.FindProperty("pointsPerTurn");
			loopProp				= serializedObject.FindProperty("loop");

			upModeProp				= serializedObject.FindProperty("upMode");
			axisProp				= serializedObject.FindProperty("axis");
			extraTwistProp			= serializedObject.FindProperty("extraTwistDegrees");
			useTurnTwistProp		= serializedObject.FindProperty("useTurnBasedTwist");

			loopBlendProp			= serializedObject.FindProperty("loopBlendTangents");
			loopForceProp			= serializedObject.FindProperty("loopForceMatch");
			generateOnValidateProp	= serializedObject.FindProperty("generateOnValidate");
			reverse					= serializedObject.FindProperty("reverse");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			//EditorGUILayout.LabelField("Helix Shape", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(radius1Prop);
			EditorGUILayout.PropertyField(radius2Prop);
			EditorGUILayout.PropertyField(heightProp);
			EditorGUILayout.PropertyField(turnsProp);
			EditorGUILayout.PropertyField(pointsPerTurnProp);
			EditorGUILayout.PropertyField(biasProp);
			EditorGUILayout.PropertyField(clockwiseProp);
			EditorGUILayout.PropertyField(loopProp);
			EditorGUILayout.PropertyField(reverse);

			EditorGUILayout.Space();
			//EditorGUILayout.LabelField("Twist & Orientation", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(upModeProp, new GUIContent("Up Vector Mode"));
			EditorGUILayout.PropertyField(axisProp);
			EditorGUILayout.PropertyField(useTurnTwistProp, new GUIContent("Use Turn-Based Twist"));
			EditorGUILayout.PropertyField(extraTwistProp, new GUIContent("Extra Twist (deg)"));

			EditorGUILayout.Space();
			//EditorGUILayout.LabelField("Loop Options", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(loopBlendProp, new GUIContent("Blend Tangents"));
			EditorGUILayout.PropertyField(loopForceProp, new GUIContent("Force Start/End Match"));

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(generateOnValidateProp, new GUIContent("Auto Regenerate"));

			EditorGUILayout.Space();
			DrawButtons();

			serializedObject.ApplyModifiedProperties();
		}

		void DrawButtons()
		{
			var script = target as HelixSplineAdvanced;

			EditorGUILayout.BeginHorizontal();
			if ( GUILayout.Button("Regenerate") )	{ Undo.RecordObject(script, "Regenerate Helix"); script.Regenerate(); EditorUtility.SetDirty(script); }
			//if ( GUILayout.Button("Reverse") )		{ Undo.RecordObject(script, "Reverse Helix"); script.Reverse(); EditorUtility.SetDirty(script); }
			if ( GUILayout.Button("Center Pivot") ) { Undo.RecordObject(script, "Center Pivot"); script.CenterPivot(); EditorUtility.SetDirty(script); }
			EditorGUILayout.EndHorizontal();
		}

		void OnSceneGUI()
		{
			var script = (HelixSplineAdvanced)target;
			Transform t = script.transform;

			Handles.color = Color.cyan;

			// Draw wire discs for radius1 and radius2
			Handles.DrawWireDisc(t.position, script.axis.normalized, script.radius1);

			Vector3 rpos2 = t.position + script.axis.normalized * script.height;
			Handles.DrawWireDisc(rpos2, script.axis.normalized, script.radius2);

			// Radius1 handle
			Vector3 right = t.TransformDirection(Vector3.right);
			Vector3 handlePosR1 = t.position + right * script.radius1;
			EditorGUI.BeginChangeCheck();
			Vector3 newR1Pos = Handles.FreeMoveHandle(handlePosR1, HandleUtility.GetHandleSize(handlePosR1) * 0.04f, Vector3.zero, Handles.SphereHandleCap);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(script, "Adjust Radius1");
				script.radius1 = Vector3.Distance(t.position, newR1Pos);
				script.Regenerate();
				EditorUtility.SetDirty(script);
			}

			// Axis rotation handle
			EditorGUI.BeginChangeCheck();
			Quaternion axisRot = Quaternion.LookRotation(script.axis.normalized, Vector3.up);
			Quaternion newRot = Handles.RotationHandle(axisRot, t.position);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(script, "Adjust Axis");
				script.axis = newRot * Vector3.forward;
				script.Regenerate();
				EditorUtility.SetDirty(script);
			}

			// Height handle
			EditorGUI.BeginChangeCheck();
			Vector3 heightHandle = t.position + script.axis.normalized * script.height;
			Vector3 newHeightPos = Handles.Slider(heightHandle, script.axis.normalized, HandleUtility.GetHandleSize(heightHandle) * 0.5f, Handles.ArrowHandleCap, 0.1f);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(script, "Adjust Height");
				script.height = Vector3.Dot(newHeightPos - t.position, script.axis.normalized);
				script.Regenerate();
				EditorUtility.SetDirty(script);
			}
		}
	}
}
#endif
