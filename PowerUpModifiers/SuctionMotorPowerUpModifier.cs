using System.Timers;
using _Scripts.Collectables;
using _Scripts.Player;
using Debug = UnityEngine.Debug;

namespace _Scripts.PowerUpModifiers
{
    public class SuctionMotorPowerUpModifier : TimeLimitedModifier<IPlayerSuctionMotorParameters>
    {
        private readonly PowerUpCollectable _collectable;
        
        public SuctionMotorPowerUpModifier(PowerUpCollectable collectable)
        {
            _collectable = collectable;
            SetExpirationTimeInSec(_collectable.ExpireAfter);
        }

        public override IPlayerSuctionMotorParameters Apply(IPlayerSuctionMotorParameters current)
        {
            var newProps =  new PlayerSuctionMotorParameters(
                current.PullInSpeed * _collectable.playerSpeedMultiplayer, 
                current.TurnedOff, 
                current.radius * _collectable.suctionRadiusMultiplyer);

            return newProps;
        }

    }
}