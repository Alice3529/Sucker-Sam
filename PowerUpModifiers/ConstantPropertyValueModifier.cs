namespace _Scripts.PowerUpModifiers
{
    public class ConstantPropertyValueModifier : PropertyValueModifier<float> {
        private readonly float _newValue;

        public ConstantPropertyValueModifier(float newValue) 
        {
            _newValue = newValue;
        }

        public override float Modify(float value)
        {
            return base.Modify(_newValue);
        }
    }
}