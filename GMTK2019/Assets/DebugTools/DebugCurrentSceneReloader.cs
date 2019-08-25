using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugCurrentSceneReloader : MonoBehaviour
{
	// -------------------------------------------------------------------------------------

	[SerializeField] private KeyCode RestartKey = KeyCode.R;

	// -------------------------------------------------------------------------------------

	private void Awake()
	{
#if !UNITY_EDITOR
		Destroy(this);
#endif // UNITY_EDITOR
	}

	private void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(RestartKey))
		{
			Debug.Log("Reload Current Scene");
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
#endif // UNITY_EDITOR
	}
}
