namespace _Scripts.Player
{
    public class PlayerMovementParameters : IPlayerMovementParameters
    {
        public float forwardSpeed { get; }
        public float rotationSpeed { get; }
        public bool IsPoweredUp { get; }

        public PlayerMovementParameters(float forwardSpeed, float rotationSpeed, bool canEatEnemy)
        {
            this.forwardSpeed = forwardSpeed;
            this.rotationSpeed = rotationSpeed;
            this.IsPoweredUp = canEatEnemy;
        }
    }
}