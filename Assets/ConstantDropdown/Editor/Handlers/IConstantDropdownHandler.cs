using System;
using UnityEditor;
using UnityEngine;

namespace DTech.ConstantDropdown.Editor
{
	public interface IConstantDropdownHandler
	{
		SerializedPropertyType ServicedPropertyType { get; }

		void RefreshMap();
		GUIContent GetDropdownCaption(Type linkedType, SerializedProperty property);
		bool TrySelectValue(SerializedProperty property, Type linkedType);
	}
}