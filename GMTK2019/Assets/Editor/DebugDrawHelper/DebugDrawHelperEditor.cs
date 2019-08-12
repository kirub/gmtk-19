using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DebugDrawHelper))]
public class DebugDrawHelperEditor : Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.PrefixLabel("Shortcuts");

		++EditorGUI.indentLevel;
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("ToggleDebugDrawKey"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("ToggleDisplayDrawablesListKey"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("SwitchDisplayTypeKey"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("DisplayShortcutsKey"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("PrevItemKey"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("NextItemKey"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("PrevPageKey"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("NextPageKey"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("ToggleSelectionKey"));
		}
		--EditorGUI.indentLevel;

		EditorGUILayout.Space();
		EditorGUILayout.PrefixLabel("Colors");

		++EditorGUI.indentLevel;
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("DefaultColor"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("SelectedColor"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("HighlightedColor"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("HighlightedSelectedColor"));
		}
		--EditorGUI.indentLevel;

		EditorGUILayout.Space();
		EditorGUILayout.PrefixLabel("Misc");

		++EditorGUI.indentLevel;
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxDrawablesPerPage"));
		}
		--EditorGUI.indentLevel;

		serializedObject.ApplyModifiedProperties();
	}
}
