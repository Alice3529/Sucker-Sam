using System;
using _Scripts.PowerUpModifiers;
using UnityEngine;

namespace _Scripts.Player.CollisionActions
{
    [RequireComponent(typeof(PlayerCollisionDetector), typeof(PlayerWasteBin))]
    public class RecyclingActionCollisionAction : MonoBehaviour
    {
        private PlayerCollisionDetector _collisionDetector;
        private PlayerWasteBin _wasteBin;
        private void Start()
        {
            _wasteBin = GetComponent<PlayerWasteBin>();
            _collisionDetector = GetComponent<PlayerCollisionDetector>();
            _collisionDetector.OnCollisionWithRecyclingStation += OnCollisionWithCollectable;
        }

        private void OnCollisionWithCollectable(RecyclingStation.RecyclingStation recyclingStation)
        {
            if (!recyclingStation) return;
    
            // put garbage from my waste bin into recycle station
            var putGarbageCount = Math.Max(_wasteBin.garbageCount, recyclingStation.freeCapacity);
            for (int i = 0; i < putGarbageCount; i++)
                recyclingStation.PutOneGarbage(_wasteBin);
        }
    }
}