using UnityEngine;

public interface IDebugDrawable
{
	/*
	 * You can put this around the code inside  DebugDraw
#if UNITY_EDITOR
#endif
	 */

	void DebugDraw(ref Rect BasePos, float TextYIncrement, GUIStyle Style);
}
