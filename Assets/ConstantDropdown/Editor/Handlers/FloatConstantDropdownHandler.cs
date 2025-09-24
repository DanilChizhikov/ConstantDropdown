using UnityEditor;

namespace DTech.ConstantDropdown.Editor
{
    internal sealed class FloatConstantDropdownHandler : ConstantDropdownHandlerT<float>
    {
        public override SerializedPropertyType ServicedPropertyType => SerializedPropertyType.Float;

        protected override float GetPropertyValue(SerializedProperty property) => property.floatValue;

        protected override void SetPropertyValue(SerializedProperty property, float value) =>
            property.floatValue = value;
    }
}