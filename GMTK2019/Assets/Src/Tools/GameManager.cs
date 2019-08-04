﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public string SceneGameName;

    public GameObject CanvasMenuStart;
    public GameObject CanvasMenuIngame;
    public GameObject CanvasMenuGameOver;
    public Text TextHighScores;
    public Text TextLatestScore;

    // Start is called before the first frame update

    public List<int> HighScores = new List<int>();
    public float DurationGameOverScreen = 5f;
    public int ScoreMultiplier = 1;
    public int MaxNumberOfHighScores = 5;
	
    public float LatestScore = 0;

	public bool IsInPause { get; private set; } = false;
	private bool IsGameOver = false;
	private float LastTimeScale = 0f;

	public class OnPauseEvent : UnityEvent<bool> { }
	public OnPauseEvent OnPauseUnpauseEvent { get; } = new OnPauseEvent();

	void LoadSceneGame()
    {
        SceneManager.LoadScene(SceneGameName);
        TextLatestScore.gameObject.SetActive(true);
    }

	private void Awake()
	{
		if (Instance)
		{
			Debug.LogError("Already an Instance of GameManager");
			Destroy(this);
			return;
		}
		Instance = this;

		DontDestroyOnLoad(gameObject);
	}
	
	void Start()
    {
        InitializeHighScores();
       // Debug.Log("Manager Start");
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsGameOver && Input.GetKeyDown(KeyCode.Escape))
        {
			if (IsInPause)
			{
				UnPause();
			}
			else
			{
				Pause();
			}
        }
        TextLatestScore.text = (((int)LatestScore) * ScoreMultiplier)+"";
    }

    public void GameStart()
    {
        Debug.Log("GameStart");
		IsInPause = false;
		CanvasMenuStart.SetActive(false);
		LoadSceneGame();
    }

    public void RestartWhileIngame()
    {
        Debug.Log("RestartWhileIngame");
        HandleHighScores(((int)LatestScore)*ScoreMultiplier);
        CanvasMenuIngame.SetActive(false);
        LoadSceneGame();
        
    }
    public void RestartWhileGameOver()
    {
        Debug.Log("RestartWhileGameOver");
        CanvasMenuStart.SetActive(false);
		CanvasMenuGameOver.SetActive(false);
        HandleHighScores(((int)LatestScore) * ScoreMultiplier);
		LoadSceneGame();
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        CanvasMenuIngame.SetActive(false);//au cas ou menu pdt jeu actif puis meurt
		CanvasMenuGameOver.SetActive(true);
		HandleHighScores(((int)LatestScore) * ScoreMultiplier);
    }

    public void Pause()
    {
		if (IsGameOver || IsInPause)
		{
			return;
		}

        Debug.Log("Pause ( a faire ?) ");
		//handle pause
		IsInPause = true;

		OnPauseUnpauseEvent.Invoke(true);

		LastTimeScale = Time.timeScale;
		Time.timeScale = 0f;

		CanvasMenuIngame.SetActive(true);
	}

    public void UnPause()
    {
		if (IsGameOver || !IsInPause)
		{
			return;
		}

        Debug.Log("UnPause ( a faire ?) ");
		//handle unpause...
		IsInPause = false;

		CanvasMenuIngame.SetActive(false);
		
		Time.timeScale = LastTimeScale;

		OnPauseUnpauseEvent.Invoke(false);
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			Pause();
		}
		else
		{
			UnPause();
		}
	}

	private void InitializeHighScores()
    {
        for (int i = 0; i < MaxNumberOfHighScores; i++)
        {
            HighScores.Add(0);
        }
        for (int i = 0; i < MaxNumberOfHighScores; i++)
        {
            TextHighScores.text += HighScores[i] + "\n";
        }
    }
    private void HandleHighScores(int newScore)
    {
        for (int i=0;i< MaxNumberOfHighScores; i++)
        {
            if (HighScores[i]<newScore)
            {
                if (i+1< MaxNumberOfHighScores)
                {
                    for (int y= MaxNumberOfHighScores-1; y>i ;y--)
                    {
                        HighScores[y] = HighScores[y - 1];
                    }
                }
                HighScores[i] = newScore;
                i = MaxNumberOfHighScores;
            }
        }
        TextHighScores.text = "";
        for (int i = 0; i < MaxNumberOfHighScores; i++)
        {
            TextHighScores.text += HighScores[i] + "\n";
        }
    }
   
    public void ExitGame()
    {
        Debug.Log("Exit sauf que ça marche que en build^^");
        Application.Quit();
    }
    

}