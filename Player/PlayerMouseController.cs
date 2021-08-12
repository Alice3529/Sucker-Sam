using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Scripts.Player
{
    /**
     * Keyboard Controller for Player
    */
    public class PlayerMouseController : MonoBehaviour
    {

        public GameObject player;
        public Camera mainCamera;

        private IPlayerMovement _playerMovement;


        private void Start()
        {
            //verify that objects are assigned properly
            if (!player)
                throw new Exception("please assign SuckerSan player that you would like to control with this script!");
            if (!mainCamera)
                throw new Exception("please assign MainCamera otherwise cannot calculate screenPosition to gameWorld position!");

            _playerMovement = player.GetComponentInChildren<IPlayerMovement>();
            
            if (_playerMovement == null)
                throw new Exception("'SuckerSum' should have component that implements IPlayerMovement interface!");

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!Input.GetMouseButtonDown(0)) return; 
            lastMousePosition = Input.mousePosition;

            //if (lastMousePosition == Vector3.zero) return;

            var playerScreenPos = this.mainCamera.WorldToScreenPoint(_playerMovement.Position);

            var distance = lastMousePosition - playerScreenPos;

            if (Math.Abs(distance.x) > Math.Abs(distance.y))
            {
                if (distance.x > 0) 
                    _playerMovement.MoveRight();
                else
                    _playerMovement.MoveLeft();
            }
            else
            {
                if(distance.y<0)
                    _playerMovement.MoveDown();
                else
                    _playerMovement.MoveUp();
            }
            
            //if (Math.Abs(distance.magnitude) < 10)
            //    lastMousePosition = Vector3.zero;
        }

        public Vector3 lastMousePosition;
    }
}