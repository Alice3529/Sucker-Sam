using UnityEngine;

namespace _Scripts.Collectables
{
    public class PowerUpCollectable : MonoBehaviour, ICollectable
    {
        public GameObject GameObject => gameObject;
        public CollectableTypeEnum Type => CollectableTypeEnum.PowerUp;
        
        public int ExpireAfter = 10;

        public float suctionPowerMultiplyer = 2;
        public float suctionRadiusMultiplyer = 2;
        public float playerSpeedMultiplayer = 2;
    }
}