using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CubeGenerator))]
public class CubeGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (Application.isPlaying)
		{
			EditorGUILayout.Space();
			
			if (EditorGUILayout.Toggle("Generate", false))
			{
				(serializedObject.targetObject as CubeGenerator).Generate();
			}
		}
	}
}
