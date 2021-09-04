using System;
using _Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts.Canvas_Scripts
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField]
        private PlayerHealth _playerHealth;

        [SerializeField] private Button PlayButton;
        [SerializeField] GameObject playerStatistics;

        private void Start()
        {
            if (!_playerHealth)
                _playerHealth = FindObjectOfType<PlayerHealth>();
        }

        private void OnEnable()
        {
            //hide play button if no lives
            if (_playerHealth.HasDied)
                PlayButton?.gameObject?.SetActive(false);
                
            
            //freeze game
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }

        public void ShowScreen() { 
           gameObject.SetActive(true);
            playerStatistics.SetActive(false);
        }
        public void HideScreen()
        {
            gameObject.SetActive(false);
            playerStatistics.SetActive(true);
        }

            public void QuitButtonClicked()
        {
            SceneManager.LoadScene(0);
            HideScreen();

        }

        public void PlayButtonClicked()
        {
            HideScreen();
        }

    }
}
