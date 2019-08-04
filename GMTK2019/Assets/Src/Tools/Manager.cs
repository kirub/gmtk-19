using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class Manager : MonoBehaviour
{
    public string SceneGame;
    public string SceneGameOver;
    public GameObject StartCam;

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

    private bool IsSceneGameLoaded = false;
    public float LatestScore = 0;
    

    IEnumerator LoadSceneGame()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneGame, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        IsSceneGameLoaded = true;
        TextLatestScore.gameObject.SetActive(true);
    }
    IEnumerator LoadSceneGameOver()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneGameOver, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        CanvasMenuGameOver.SetActive(true);
    }


    IEnumerator UnLoadSceneGame()
    {
        AsyncOperation asyncUnLoad = SceneManager.UnloadSceneAsync(SceneGame);
        while (!asyncUnLoad.isDone)
        {
            yield return null;
        }
    }
    IEnumerator UnLoadSceneGameOver()
    {
        AsyncOperation asyncUnLoad = SceneManager.UnloadSceneAsync(SceneGameOver);
        while (!asyncUnLoad.isDone)
        {
            yield return null;
        }
        StartCam.SetActive(true);
        CanvasMenuGameOver.SetActive(false);
    }


    void Start()
    {
        InitializeHighScores();
       // Debug.Log("Manager Start");
    }

    // Update is called once per frame
    void Update()
    {
        if(IsSceneGameLoaded == true)
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
        TextLatestScore.text = (((int)LatestScore) * ScoreMultiplier)+"";
    }

    public void GameStart()
    {
        Debug.Log("GameStart");
        StartCam.SetActive(false);
        StartCoroutine(LoadSceneGame());
        CanvasMenuStart.SetActive(false);
    }

    public void RestartWhileIngame()
    {
        Debug.Log("RestartWhileIngame");
        HandleHighScores(((int)LatestScore)*ScoreMultiplier);
        IsSceneGameLoaded = false;
        CanvasMenuIngame.SetActive(false);
        StartCoroutine(UnLoadSceneGame());
        StartCoroutine(LoadSceneGame());
        
    }
    public void RestartWhileGameOver()
    {
        CanvasMenuStart.SetActive(false);
        Debug.Log("RestartWhileGameOver");
        HandleHighScores(((int)LatestScore) * ScoreMultiplier);
        StartCoroutine(UnLoadSceneGameOver());
        StartCoroutine(LoadSceneGame());
        CanvasMenuGameOver.SetActive(false);
    }

    public void GameOver()
    {
        CanvasMenuIngame.SetActive(false);//au cas ou menu pdt jeu actif puis meurt
        HandleHighScores(((int)LatestScore) * ScoreMultiplier);
        IsSceneGameLoaded = false;
        StartCoroutine(UnLoadSceneGame());
        StartCoroutine(LoadSceneGameOver());
        Debug.Log("GameOver");
    }

    public void Pause()
    {
        Debug.Log("Pause ( a faire ?) ");
        //handle pause
        CanvasMenuIngame.SetActive(true);
    }

    public void UnPause()
    {
        Debug.Log("UnPause ( a faire ?) ");
        //handle unpause...
        CanvasMenuIngame.SetActive(false);
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
        EditorApplication.isPlaying = false;
    }
    

}
