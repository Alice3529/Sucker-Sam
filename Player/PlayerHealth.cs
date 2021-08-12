using System;
using _Scripts.Canvas_Scripts;
using TMPro;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] int maxHealth=3;
        [SerializeField] GameOverScreen loseCanvas;
        [SerializeField] TextMeshProUGUI healthText;

        private int currentHealth;
        
        public event Action OnGameOver;
        public event Action OnLooseLive;

        public bool HasDied => currentHealth < 1;

        private void Start()
        {
            currentHealth = maxHealth;
            
            if (loseCanvas == null)
                loseCanvas = FindObjectOfType<GameOverScreen>();

            if (!loseCanvas)
                throw new Exception("GameOverScreen was not found in the scene");
        }

        public void MinusHealth(int amount)
        {
            if (HasDied) return;
            
            currentHealth -= 1;
            //rise game over
            OnLooseLive?.Invoke();
            
            if (HasDied)
            {
                currentHealth = 0;
                OnGameOver?.Invoke();
                
            }
            healthText.text = currentHealth.ToString();
            loseCanvas.ShowScreen();
        }
    }
}
 