using System;
using UnityEngine;

namespace _Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerPoweredUpChangeAnimation : MonoBehaviour
    {
        public RuntimeAnimatorController standardController;
        public RuntimeAnimatorController poweredUpController;
        public RuntimeAnimatorController activeController;
        
        private IPlayerMovement _movement;
        private Animator _animator;
        private void Start()
        {
            _movement = transform.parent.GetComponent<IPlayerMovement>();
            if (_movement == null) throw new Exception("This object must be under parent GameObject that contains IPlayerMovement implementation!");
            if (!standardController || !poweredUpController)
                throw new Exception("Please define start/poweredUp animation Controller!");
            
            _animator = GetComponentInChildren<Animator>();
            _movement.OnIsPoweredUpChanged += PlayerPoweredUpChanged;
            activeController = standardController;
            ChangeAnimatorController(false);
        }

        private void OnDestroy()
        {
            _movement.OnIsPoweredUpChanged -= PlayerPoweredUpChanged;
        }

        private void PlayerPoweredUpChanged(bool IsPoweredUp, float RemainingTime)
        {
            ChangeAnimatorController(IsPoweredUp);
        }

        private void ChangeAnimatorController(bool isPoweredUp)
        {
            if (isPoweredUp && activeController != poweredUpController)
                _animator.runtimeAnimatorController = activeController = poweredUpController;
            
            if (!isPoweredUp && activeController != standardController)
                _animator.runtimeAnimatorController = activeController = standardController;
        }
    }
}