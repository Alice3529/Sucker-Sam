namespace _Scripts.Player
{
    public interface IPropertyModifier<T>
    {
        void Initialize(T currentValue);
        T Apply(T currentValue);
        bool IsExpired { get; }
        float TimeToExpire { get; set; }
    }
}