using TMPro;
using UnityEngine;

namespace _Scripts.Player
{
    [RequireComponent(typeof(IPlayerMovement))]
    public class PlayerUpdateStatsScreen : MonoBehaviour
    {
        public TMP_Text PoweredUpRemainingTimeField;
        
        private IPlayerMovement _movement;
        private Animator _animtator;
        private float currentTime;

        private void Start()
        {
            _movement = GetComponent<IPlayerMovement>();
            _animtator = GetComponentInChildren<Animator>();
            _movement.OnIsPoweredUpChanged += PlayerPoweredUpChanged;
        }

        private void OnDestroy()
        {
            _movement.OnIsPoweredUpChanged -= PlayerPoweredUpChanged;
        }

        private void PlayerPoweredUpChanged(bool IsPoweredUp, float RemainingTime)
        {
            currentTime = RemainingTime;
            UpdateStatsScreen(RemainingTime);
        }

        private void UpdateStatsScreen(float RemainingTime)
        {
            PoweredUpRemainingTimeField.text = RemainingTime.ToString();
        }

        public bool IsTimeOver()
        {
            if (currentTime <= 0)
            {
                return true;
            }
            return false;
        }
    }
}