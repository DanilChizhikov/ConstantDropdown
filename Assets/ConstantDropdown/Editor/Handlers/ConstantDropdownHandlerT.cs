using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DTech.ConstantDropdown.Editor
{
	public abstract class ConstantDropdownHandlerT<T> : IConstantDropdownHandler
	{
		private readonly ConstMap<T> _map = new();
		
		public abstract SerializedPropertyType ServicedPropertyType { get; }

		public void RefreshMap() => _map.Clear();

		public bool Draw(ConstantDrawInfo info)
		{
			if (!_map.TryGetSourceType(info.Attribute.LinkingType, out Dictionary<string, T> map, out GUIContent[] content))
			{
				return false;
			}

			T propertyValue = GetPropertyValue(info.Property);
			int selectedIndex = ConstantDropdownUtils.GetSelectedIndex(propertyValue, map, content);
			using var check = new EditorGUI.ChangeCheckScope();
			selectedIndex = EditorGUI.Popup(info.Position, info.Label, selectedIndex, content);
			if (check.changed &&
				selectedIndex >= 0 &&
				selectedIndex < content.Length &&
				map.TryGetValue(content[selectedIndex].text, out T value))
			{
				SetPropertyValue(info.Property, value);
			}
			
			ConstantDropdownUtils.DrawErrorIfInvalid(info.Position, selectedIndex, GetPropertyValue(info.Property).ToString());
			return true;
		}

		protected abstract T GetPropertyValue(SerializedProperty property);

		protected abstract void SetPropertyValue(SerializedProperty property, T value);
	}
}