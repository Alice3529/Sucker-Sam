using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    private CanvasActions _canvasActions;
    
    // Start is called before the first frame update
    void Start()
    {
        _canvasActions = FindObjectOfType<CanvasActions>();
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }

    public void PlayButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
    public void CancelButtonClicked()
    {
        SceneManager.LoadScene(0);
    }

    public void Show()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }
}
