namespace _Scripts.Player
{
    public interface IGarbageContainer
    {
        bool HasGarbage { get; }
        void GetOutOneGarbage();
    }
}