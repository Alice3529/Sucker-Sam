using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasActions : MonoBehaviour
{
    private GameObject pauseObject;
    private GameObject normalObject;
    private Canvas pauseCanvas;
    private Canvas normalCanvas;

    private static CanvasActions _instance;
    public CanvasActions Instance => _instance;


    void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else 
            Destroy(gameObject);
        
    }
    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    void Update()
    {
       
        if (pauseCanvas == null)
        {
            if (GameObject.Find("GoodUICanvas") != null)
            {
                normalObject = GameObject.Find("GoodUICanvas");
                normalCanvas = normalObject.GetComponentInChildren<Canvas>();
            }
           
        }
        if (pauseCanvas == null)
        {
            if (GameObject.Find("PauseCanvas") != null)
            {
                pauseObject = GameObject.Find("PauseCanvas");
                pauseCanvas = pauseObject.GetComponentInChildren<Canvas>();
            }

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseCanvas.enabled = true;
            normalCanvas.enabled = false;
            Time.timeScale = 0f;
        }
    }
    public void ContinueButton()
    {
        Time.timeScale = 1f;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Pause()
    {
        Time.timeScale = 0f;

    }

}
