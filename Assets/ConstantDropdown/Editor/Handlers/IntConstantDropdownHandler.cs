using UnityEditor;

namespace DTech.ConstantDropdown.Editor
{
	internal sealed class IntConstantDropdownHandler : ConstantDropdownHandlerT<int>
	{
		public override SerializedPropertyType ServicedPropertyType => SerializedPropertyType.Integer;
		
		protected override int GetPropertyValue(SerializedProperty property) => property.intValue;
		
		protected override void SetPropertyValue(SerializedProperty property, int value) => property.intValue = value;
	}
}