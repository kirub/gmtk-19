using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
	[SerializeField] private List<TutorialPage> Pages = new List<TutorialPage>();
	[SerializeField] GameObject PrevBtn = null;

	private int CurrentPage = -1;

	public bool IsStarted { get { return CurrentPage >= 0; } }
	public bool IsLastPage { get { return CurrentPage == Pages.Count - 1; } }
	public bool IsFinished { get { return CurrentPage >= Pages.Count; } }

	private void OnEnable()
	{
		CurrentPage = -1;
		PrevBtn.SetActive(false);
		NextInfo();
	}

	void CheckIsFinishedOrDisplayPage()
	{
		if (!IsStarted)
		{
			BackToMenu();
		}
		else if (IsFinished)
		{
			ExitTutorial();
		}
		else
		{
			Pages[CurrentPage].gameObject.SetActive(true);
			PrevBtn.SetActive(CurrentPage > 0);
		}
	}

	public void PreviousInfo()
	{
		if (IsStarted && !IsFinished)
		{
			Pages[CurrentPage].gameObject.SetActive(false);
			--CurrentPage;
			CheckIsFinishedOrDisplayPage();
		}
	}
	public void NextInfo()
	{
		if (IsStarted)
		{
			if (Pages[CurrentPage].Next())
			{
				if (IsLastPage)
				{
					ExitTutorial();
				}
				else
				{
					Pages[CurrentPage].gameObject.SetActive(false);
					++CurrentPage;
					CheckIsFinishedOrDisplayPage();
				}
			}
		}
		else
		{
			CurrentPage = 0;
			CheckIsFinishedOrDisplayPage();
		}
	}

	public void BackToMenu()
	{
		if (GameManager.Instance)
		{
			GameManager.Instance.TutorialStop();
		}
	}

	public void ExitTutorial()
	{
		if (GameManager.Instance)
		{
			GameManager.Instance.GameStart();
		}
	}
}
