using System;
using UnityEngine;

namespace _Scripts.Player
{
    /**
     * Keyboard Controller for Player
    */
    public class PlayerKeyboardController : MonoBehaviour
    {
        public GameObject player;
        private IPlayerMovement _playerMovement;
       

        private void Start()
        {
            //verify that objects are assigned properly
            if (!player)
                throw new Exception("please assign SuckerSan player that you would like to control with this script!");

            _playerMovement = player.GetComponentInChildren<IPlayerMovement>();
            
            if (_playerMovement == null)
                throw new Exception("'SuckerSum' should have component that implements IPlayerMovement interface!");

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                _playerMovement.MoveLeft();
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                _playerMovement.MoveDown();
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                _playerMovement.MoveRight();
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                _playerMovement.MoveUp();
        }

       

        
    }
}