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
            int[] wallsDirections = FindObjectOfType<SearchWalls>().GetDirections();//l,r,u,b
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && (wallsDirections[0]!=1)) 
                _playerMovement.MoveLeft();
            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && (wallsDirections[3] != 1))
                _playerMovement.MoveDown();
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && (wallsDirections[1] != 1))
                _playerMovement.MoveRight();
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && (wallsDirections[2] != 1))
                _playerMovement.MoveUp();
        }

       

        
    }
}