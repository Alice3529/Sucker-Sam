namespace _Scripts.Player
{
    public class PlayerSuctionMotorParameters : IPlayerSuctionMotorParameters
    {
        public PlayerSuctionMotorParameters(float pullInSpeed, bool turnedOff, float radius)
        {
            PullInSpeed = pullInSpeed;
            TurnedOff = turnedOff;
            this.radius = radius;
        }

        public float PullInSpeed { get; }
        public bool TurnedOff { get; }
        public float radius { get; }
    }
}