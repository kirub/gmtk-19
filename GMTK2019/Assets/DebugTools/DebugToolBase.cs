using UnityEngine;
using System.Collections;

public abstract class DebugToolBase : MonoBehaviour
{
	private void Awake()
	{
#if !UNITY_EDITOR
		Destroy(this);
#endif // UNITY_EDITOR
	}
}
