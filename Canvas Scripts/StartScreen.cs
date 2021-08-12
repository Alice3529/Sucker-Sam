using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private CanvasActions _canvasActions;
    // Start is called before the first frame update
    void Start()
    {
        _canvasActions = FindObjectOfType<CanvasActions>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonClicked()
    {
        _canvasActions.StartGame();
    }
    
}
