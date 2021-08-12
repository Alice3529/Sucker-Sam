namespace _Scripts.Player
{
    public interface IPlayerMovementParameters
    {
        float forwardSpeed { get; }
        float rotationSpeed { get; }
        bool IsPoweredUp { get; }
    }
}