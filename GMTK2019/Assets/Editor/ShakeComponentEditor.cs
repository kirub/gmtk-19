using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShakeComponent))]
public class ShakeComponentEditor : Editor
{
	private float DebugShakeTime = 1f;
	private float DebugShakeAmount = 5f;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (Application.isPlaying)
		{
			EditorGUILayout.Space();

			DebugShakeTime = EditorGUILayout.FloatField("Debug Time", DebugShakeTime);
			DebugShakeAmount = EditorGUILayout.FloatField("Debug Amount", DebugShakeAmount);
			if (EditorGUILayout.Toggle("Force Shake", false))
			{
				(serializedObject.targetObject as ShakeComponent).ShakeCamera(DebugShakeTime, DebugShakeAmount);
			}
		}
	}
}
