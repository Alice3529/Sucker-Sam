using System;
using _Scripts.Player;
using TMPro;
using UnityEngine;
using Enemy;

namespace _Scripts.Canvas_Scripts
{
    public class StartGameTimer : MonoBehaviour
    {
        [SerializeField] float timer=3;
        int currentTime;
        [SerializeField] TextMeshProUGUI timeText;
        [SerializeField] Canvas goodUICanvas;
        [SerializeField] Canvas timerCanvas;
        [SerializeField] GameObject enemiesList;
        [SerializeField] GameObject player;

        public event Action<int> OnStartGameTimerChanged;

        void Start()
        {
            DisableEnemies();
            DisablePlayer();

            currentTime = 3;
            RiseTimeChanged();
        }

        private void DisableEnemies()
        {
            for (var i = 0; i < enemiesList.transform.childCount; i++)
            {
                enemiesList.transform.GetChild(i).GetComponent<EnemyAI1>().enabled = false;
            }
        }

        void Update()
        {
            if (timer < 0) return;
            timer -= Time.deltaTime;
            
            currentTime = (int) Math.Round(timer, 0);
            RiseTimeChanged();
            if (currentTime <= 0) StartGame();
        }

        private void RiseTimeChanged()
        {
            if (lastReportedTime == currentTime) return;
            
            lastReportedTime = currentTime;
            timeText.text = currentTime.ToString();
            OnStartGameTimerChanged?.Invoke(currentTime);
        }

        public int lastReportedTime { get; set; }

        private void DisablePlayer()
        {
            goodUICanvas.enabled = false;
            timerCanvas.enabled = true;
            player.GetComponent<PlayerMovememt>().enabled = false;
        }

        private void StartGame()
        {
            goodUICanvas.enabled = true;
            timerCanvas.enabled = false;
            for (int i = 0; i < enemiesList.transform.childCount; i++)
            {
                enemiesList.transform.GetChild(i).GetComponent<EnemyAI1>().enabled = true;
            }

            player.GetComponent<PlayerMovememt>().enabled = true;
        }
    }
}
