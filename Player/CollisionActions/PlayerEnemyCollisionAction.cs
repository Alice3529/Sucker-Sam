using System;
using UnityEngine;

namespace _Scripts.Player.CollisionActions
{
    [RequireComponent(typeof(PlayerCollisionDetector), typeof(PlayerMovememt))]
    public class PlayerEnemyCollisionAction : MonoBehaviour
    {
        private PlayerCollisionDetector _collisionDetector;
        private PlayerMovememt _playerMovement;
        EnemyAI1[] enemies = new EnemyAI1[4];
        private void Start()
        {
            _collisionDetector = GetComponent<PlayerCollisionDetector>();
            _playerMovement = GetComponent<PlayerMovememt>();
            _collisionDetector.OnCollisionWithEnemy += OnCollisionWithCollectable;
            enemies = FindObjectsOfType<EnemyAI1>();

        }

        private void OnCollisionWithCollectable(Enemy.Enemy enemy)
        {
            if (enemy.GetComponent<EnemyAI1>().IsInFrightenedMode())
            {
                enemy.GetComponent<EnemyAI1>().AppearAgain();
                GetComponent<Points>().AddPoints(enemy.Points);
                _playerMovement.ConsumedEnemy(enemy);
            }
            else
            {
                foreach (EnemyAI1 enemyAI in enemies)
                {
                    enemyAI.StartAgain();
                }
                GetComponent<PlayerMovememt>().Respawn();
                GetComponent<PlayerHealth>().MinusHealth(1);
                _playerMovement.SetTime();
               // _playerMovement.KilledBy(enemy);
               

            }
        }
    }
}