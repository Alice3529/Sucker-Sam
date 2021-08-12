using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseScreen : MonoBehaviour
{
    private CanvasActions _canvasActions;

    private void Start()
    {
        _canvasActions = FindObjectOfType<CanvasActions>();
    }

    public void ContinuePlay()
    {
        _canvasActions.ContinueButton();
    }
    public void RestartGame()
    {
        _canvasActions.MainMenu();
    }
}
