using _Scripts.Collectables;
using _Scripts.PowerUpModifiers;
using UnityEngine;

namespace _Scripts.Player.CollisionActions
{
    [RequireComponent(typeof(PlayerCollisionDetector), typeof(Points))]
    public class PlayerPowerUpCollisionAction : MonoBehaviour
    {
        private PlayerCollisionDetector _collisionDetector;
        private void Start()
        {
            _collisionDetector = GetComponent<PlayerCollisionDetector>();
            _collisionDetector.OnCollisionWithCollectable += OnCollisionWithCollectable;
        }

        private void OnCollisionWithCollectable(ICollectable collectableItem)
        {
            if (collectableItem.Type != CollectableTypeEnum.PowerUp) return;
            
            var powerUp = collectableItem as PowerUpCollectable;
            if (!powerUp) return;
            
            GetComponent<PlayerSuctionMotor>().ApplyModifier(new SuctionMotorPowerUpModifier(powerUp));
            GetComponent<PlayerMovememt>().ApplyModifier(new PlayerMovemenetPowerUpModifier(powerUp));
            EnemyAI1[] enemies = FindObjectsOfType<EnemyAI1>();
            foreach (EnemyAI1 enemy in enemies)
            {
                enemy.SetFrightenedMode();
            }
            Destroy(collectableItem.GameObject);
        }
    }
}