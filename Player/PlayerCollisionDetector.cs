using System;
using _Scripts.Collectables;
using UnityEngine;

namespace _Scripts.Player
{
    /**
     * Script responsible for collision detection with other object like maze, enemy, garbage
     */
    public class PlayerCollisionDetector : MonoBehaviour
    {
        public Action<ICollectable> OnCollisionWithCollectable;
        public Action<Enemy.Enemy> OnCollisionWithEnemy;
        public Action<RecyclingStation.RecyclingStation> OnCollisionWithRecyclingStation;
        float powerupTime=0f;
        //public Action<

        private void OnCollisionEnter2D(Collision2D other)
        {
            detectCollisionAndRaiseEvents(other.gameObject);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            detectCollisionAndRaiseEvents(col.gameObject);
        }

        private void detectCollisionAndRaiseEvents(GameObject col)
        {
            collisionWithCollectable(col);
            collisionWithEnemy(col);
            collisionWithRecyclingStation(col);
        }

        private void collisionWithRecyclingStation(GameObject col)
        {
            var collectableItem = col.GetComponent<RecyclingStation.RecyclingStation>();
            if (collectableItem == null) return;
            this.OnCollisionWithRecyclingStation?.Invoke(collectableItem);
        }

        private void collisionWithCollectable(GameObject col)
        {
            var collectableItem = col.GetComponent<ICollectable>();
            if (collectableItem == null) return;
            if (col.GetComponent<PowerUpCollectable>()!=null) {
                powerupTime = col.GetComponent<PowerUpCollectable>().ExpireAfter;
            }
            this.OnCollisionWithCollectable?.Invoke(collectableItem);
        }
        
        private void collisionWithEnemy(GameObject col)
        {
            if (!col.GetComponent<Enemy.Enemy>()) return;
            var enemy = col.GetComponent<Enemy.Enemy>();
            this.OnCollisionWithEnemy?.Invoke(enemy);
        }
        public float GetPowerupTime()
        {
            return powerupTime;
        }
    }
}


    
