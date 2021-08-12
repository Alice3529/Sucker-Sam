using _Scripts.Player;

namespace _Scripts.PowerUpModifiers
{
    public class PropertyValue<T>
    {
        private T _value;
        private PropertyValueModifier<T> _nextModifier;


        public T Value
        {
            get
            {
                if (_nextModifier == null) return _value;
                return _nextModifier.Modify(_value);
            }
            set
            {
                if (Equals(_value, value)) return;
                _value = value;
            }
        }

        public PropertyValue(T value)
        {
            _value = value;
        }

        public void AddModifier(PropertyValueModifier<T> next)
        {
            if (_nextModifier != null)
                _nextModifier.Add(next);
            else
                _nextModifier = next;
        }

        public void ClearModifier()
        {
            _nextModifier = null;
        }
    }

}