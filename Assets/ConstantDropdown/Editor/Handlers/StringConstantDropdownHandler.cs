using UnityEditor;

namespace DTech.ConstantDropdown.Editor
{
	internal sealed class StringConstantDropdownHandler : ConstantDropdownHandlerT<string>
	{
		public override SerializedPropertyType ServicedPropertyType => SerializedPropertyType.String;
		
		protected override string GetPropertyValue(SerializedProperty property) => property.stringValue;

		protected override void SetPropertyValue(SerializedProperty property, string value) => property.stringValue = value;
	}
}