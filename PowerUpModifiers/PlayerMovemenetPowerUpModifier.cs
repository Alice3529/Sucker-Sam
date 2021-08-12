using _Scripts.Collectables;
using _Scripts.Player;
using UnityEngine;

namespace _Scripts.PowerUpModifiers
{
    public class PlayerMovemenetPowerUpModifier : TimeLimitedModifier<IPlayerMovementParameters>
    {
        private readonly PowerUpCollectable _collectable;
        
        public PlayerMovemenetPowerUpModifier(PowerUpCollectable collectable)
        {
            _collectable = collectable;
            SetExpirationTimeInSec(_collectable.ExpireAfter);
        }



        public override IPlayerMovementParameters Apply(IPlayerMovementParameters currentValue)
        {
            return new PlayerMovementParameters(
                currentValue.forwardSpeed * this._collectable.playerSpeedMultiplayer,
                currentValue.rotationSpeed * this._collectable.playerSpeedMultiplayer,
                true
            );
        }
    }
}