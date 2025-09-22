using UnityEditor;
using UnityEngine;

namespace DTech.ConstantDropdown.Editor
{
	public struct ConstantDrawInfo
	{
		public Rect Position { get; }
		public SerializedProperty Property { get; }
		public ConstantDropdownAttribute Attribute { get; }
		public GUIContent Label { get; }

		public ConstantDrawInfo(Rect position,
			SerializedProperty property,
			ConstantDropdownAttribute attribute,
			GUIContent label)
		{
			Position = position;
			Property = property;
			Attribute = attribute;
			Label = label;
		}
	}
}