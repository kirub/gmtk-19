using System.Collections;
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
    public GameObject ResumeButton;
    public GameObject SoundButton;
    public GameObject CanvasHighScores;
	public Text TextHighScores;

	public AudioSource StartGameSound = null;

	[SerializeField] private AudioSource AmbientMainMenu = null;
	[SerializeField] private AudioSource AmbientInGame = null;

	private float AmbientMainMenuVolume = 0f;
	private float AmbientInGameVolume = 0f;

	// Start is called before the first frame update

	public List<int> HighScores = new List<int>();
    public float DurationGameOverScreen = 5f;
    public int ScoreMultiplier = 1;
    public int MaxNumberOfHighScores = 5;
	
    public float LatestScore = 0;

	public bool IsInPause { get; private set; } = false;
	private bool IsGameOver = false;
	private float LastTimeScale = 0f;

	private bool IsMute = false;
    private string SoundButtonText = "ON";
    

	public class OnPauseEvent : UnityEvent<bool> { }
	public OnPauseEvent OnPauseUnpauseEvent { get; } = new OnPauseEvent();

	void LoadSceneGame()
    {
        SceneManager.LoadScene(SceneGameName);
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

		CanvasMenuStart.SetActive(true);
		CanvasMenuIngame.SetActive(false);
		CanvasHighScores.SetActive(false);

		if (AmbientMainMenu)
		{
			AmbientMainMenu.Play();
			AmbientMainMenuVolume = AmbientMainMenu.volume;
		}
		if (AmbientInGame)
		{
			AmbientInGame.Stop();
			AmbientInGameVolume = AmbientInGame.volume;
		}
}
	
	void Start()
    {
        InitializeHighScores();
		// Debug.Log("Manager Start");
	}

	void UpdatePause()
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
	}

	void UpdateMute()
	{
		if (Input.GetKeyDown(KeyCode.M))
		{
			IsMute = !IsMute;
			if (IsMute)
			{
                SoundButtonText = "OFF";
                if (AmbientMainMenu)
				{
					AmbientMainMenu.volume = 0f;
				}
				if (AmbientInGame)
				{
					AmbientInGame.volume = 0f;
				}
			}
			else
			{
                SoundButtonText = "ON";
                if (AmbientMainMenu)
				{
					AmbientMainMenu.volume = AmbientMainMenuVolume;
				}
				if (AmbientInGame)
				{
					AmbientInGame.volume = AmbientInGameVolume;
				}
			}
		}
	}

	void Update()
    {
		UpdatePause();
		UpdateMute();
    }

    public void GameStart()
    {
        Debug.Log("GameStart");
		IsInPause = false;
		CanvasHighScores.SetActive(false);
		CanvasMenuStart.SetActive(false);
		CanvasMenuIngame.SetActive(false);

		if (AmbientMainMenu)
		{
			AmbientMainMenu.Stop();
		}
		if (AmbientInGame)
		{
			AmbientInGame.Play();
		}

		if (StartGameSound)
		{
			StartGameSound.Play();
		}

		LoadSceneGame();
    }

    public void GameRestart()
    {
        Debug.Log("GameRestart");
		GameStart();        
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
		CanvasHighScores.SetActive(true);
		ResumeButton.SetActive(false);
		CanvasMenuIngame.SetActive(true);
		if (Supernova.Instance)
		{
			LatestScore = Supernova.Instance.GetPlayerDistance() * 100;
		}
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

		CanvasHighScores.SetActive(true);
		ResumeButton.SetActive(true);
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

		CanvasHighScores.SetActive(false);
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
