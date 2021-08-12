using _Scripts.Collectables;
using UnityEngine;

namespace _Scripts.Player.CollisionActions
{
    [RequireComponent(typeof(PlayerCollisionDetector), typeof(PlayerSuctionMotor), typeof(PlayerMovememt))]
    public class PlayerDotCollisionAction : MonoBehaviour
    {
        private PlayerCollisionDetector _collisionDetector;
        private PlayerMovememt _playerMovement;

        private void Start()
        {
            _collisionDetector = GetComponent<PlayerCollisionDetector>();
            _playerMovement = GetComponent<PlayerMovememt>();
            _collisionDetector.OnCollisionWithCollectable += ConsumeCollectable;
        }

        public void ConsumeCollectable(ICollectable collectableItem)
        {
            //action to get points (if collectable is dot)
            if (collectableItem.Type != CollectableTypeEnum.DotOrGarbage) return;
            
            var dot = collectableItem as Dot; 
            GetComponent<Points>().AddPoints(dot.GetPoints());
            _playerMovement.RaiseConsumedCollectable(collectableItem);
            Destroy(collectableItem.GameObject);
        }
    }
}