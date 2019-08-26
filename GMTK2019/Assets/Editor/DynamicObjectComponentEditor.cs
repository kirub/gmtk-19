using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DynamicObjectComponent))]
public class DynamicObjectComponentEditor : Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.PropertyField(serializedObject.FindProperty("DestroyAfterMaxRange"));

		SerializedProperty TargetTypeProp = serializedObject.FindProperty("TargetType");
		EditorGUILayout.PropertyField(TargetTypeProp);

		DynamicObjectComponent.ETargetType Type = (DynamicObjectComponent.ETargetType)TargetTypeProp.intValue;
		if (Type != DynamicObjectComponent.ETargetType.FollowingForward)
		{
			EditorGUILayout.Space();
			EditorGUILayout.PrefixLabel("Homing Infos");

			++EditorGUI.indentLevel;
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("HomingValue"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("HomingLookAheadTime"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("HomingLookAheadMinTime"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("HomingTranslateToUseMaxSpeed"));
			}
			--EditorGUI.indentLevel;
		}

		serializedObject.ApplyModifiedProperties();
	}
}
