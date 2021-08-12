using System;
using System.Runtime.CompilerServices;
using _Scripts.Collectables;
using UnityEngine;

namespace _Scripts.Player.CollisionActions
{
    [RequireComponent(typeof(PlayerCollisionDetector), typeof(PlayerWasteBin), typeof(Points))]
    public class PlayerGarbageCollisionAction : MonoBehaviour
    {
        private PlayerCollisionDetector _collisionDetector;
        private PlayerWasteBin _wasteBin;
        private PlayerMovememt _playerMovement;
        private void Start()
        {
            _wasteBin = GetComponent<PlayerWasteBin>();
            _playerMovement = GetComponent<PlayerMovememt>();
            _collisionDetector = GetComponent<PlayerCollisionDetector>();
            _collisionDetector.OnCollisionWithCollectable += OnCollisionWithCollectable;

            if (!_playerMovement) throw new Exception("Please assign Player Movement script");
        }

        private void OnCollisionWithCollectable(ICollectable collectableItem)
        {
            //action to get points (if collectable is dot)
            if (collectableItem.Type != CollectableTypeEnum.Garbage) return;
            
            //check if garbage can be consumed
            if (_wasteBin.IsFull) return;
            
            //consume garbage
            _wasteBin.ConsumeWaste(collectableItem);
            
            //add extra points
            var dot = collectableItem as Garbage; 
            GetComponent<Points>().AddPoints(dot.GetPoints());
            _playerMovement.RaiseConsumedCollectable(collectableItem);
            Destroy(collectableItem.GameObject);
            
            
        }
    }
}