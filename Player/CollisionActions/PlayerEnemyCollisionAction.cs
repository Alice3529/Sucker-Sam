using System;
using UnityEngine;

namespace _Scripts.Player.CollisionActions
{
    [RequireComponent(typeof(PlayerCollisionDetector), typeof(PlayerMovememt))]
    public class PlayerEnemyCollisionAction : MonoBehaviour
    {
        private PlayerCollisionDetector _collisionDetector;
        private PlayerMovememt _playerMovement;
        private void Start()
        {
            _collisionDetector = GetComponent<PlayerCollisionDetector>();
            _playerMovement = GetComponent<PlayerMovememt>();
            _collisionDetector.OnCollisionWithEnemy += OnCollisionWithCollectable;
        }

        private void OnCollisionWithCollectable(Enemy.Enemy enemy)
        {
            if (!_playerMovement.IsPoweredUp || !enemy.GetComponent<EnemyAI1>().IsInFrightenedMode())
            {
                _playerMovement.KilledBy(enemy);
                return;
            }
            
            GetComponent<Points>().AddPoints(enemy.Points);
            _playerMovement.ConsumedEnemy(enemy);
            enemy.GetComponent<EnemyAI1>().AppearAgain();
            //Destroy(enemy.gameObject);

        }
    }
}