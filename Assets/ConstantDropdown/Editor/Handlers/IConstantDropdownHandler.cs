using System;
using UnityEditor;
using UnityEngine;

namespace DTech.ConstantDropdown.Editor
{
	public interface IConstantDropdownHandler
	{
		SerializedPropertyType ServicedPropertyType { get; }

		void RefreshMap();
		GUIContent GetDropdownCaption(Type linkedType, SerializedProperty property, string prefixName);
		bool TrySelectValue(SerializedProperty property, Type linkedType);
	}
}