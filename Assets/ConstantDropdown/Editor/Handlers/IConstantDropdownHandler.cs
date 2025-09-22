using UnityEditor;
using UnityEngine;

namespace DTech.ConstantDropdown.Editor
{
	public interface IConstantDropdownHandler
	{
		SerializedPropertyType ServicedPropertyType { get; }

		void RefreshMap();
		GUIContent GetCurrentValue(SerializedProperty property);
		bool TryGetProvider(SerializedProperty property, ConstantDropdownAttribute attribute, out ConstantSearchProvider provider);
	}
}