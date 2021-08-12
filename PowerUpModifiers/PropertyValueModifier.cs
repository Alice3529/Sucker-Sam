namespace _Scripts.PowerUpModifiers {

    public class PropertyValueModifier<T>
    {
        private PropertyValueModifier<T> _next;
        private bool _disabled = false;

        public void Disable()
        {
            _disabled = true;
        }
        public void Enable()
        {
            _disabled = false;
        }

        public void Add(PropertyValueModifier<T> modifier)
        {
            if (_next != null) 
                _next.Add(modifier);
            else
                _next = modifier;

        }
        public virtual T Modify(T value)
        {
            var _nextActiveModifier = GetNextActiveModifier();
            if (_nextActiveModifier == null) return default(T);

            return _nextActiveModifier.Modify(value);
        }

        private PropertyValueModifier<T> GetNextActiveModifier()
        {
            if (!_next._disabled) return _next;
            return _next?.GetNextActiveModifier();
        }
    }

    /*
    public static class ModifierTest {
        public static void test()
        {
            //initial value
            var pv = new PropertyValue<float>(0f);
            pv.Value = 5;
            
            pv.AddModifier(new ConstantValueModifier(5));
            pv.ClearModifier();
            
            //should be 5
            var v = pv.Value;
        }
    }*/
}