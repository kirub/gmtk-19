using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPage : MonoBehaviour
{
	[SerializeField] List<GameObject> ToDisplay = new List<GameObject>();

	private int CurrentDisplay = -1;

	public bool IsStarted { get { return CurrentDisplay >= 0; } }
	public bool IsFinished { get { return CurrentDisplay >= ToDisplay.Count; } }

	private void OnEnable()
	{
		CurrentDisplay = -1;

		foreach (GameObject Display in ToDisplay)
		{
			Display.SetActive(false);
		}

		Next();
	}

	public bool Next()
	{
		if (!IsFinished)
		{
			++CurrentDisplay;

			if (!IsFinished)
			{
				ToDisplay[CurrentDisplay].SetActive(true);
			}
		}

		return IsFinished;
	}
}
