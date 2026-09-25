using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DTech.ConstantDropdown.Editor
{
	public static class ConstantDropdownUtils
	{
		private const string Space = " ";

		public static void DrawErrorIfInvalid(Rect position, int selectedIndex, string currentValue)
		{
			if (selectedIndex != -1)
			{
				return;
			}

			var cachedColor = GUI.contentColor;
			GUI.contentColor = Color.red;
			EditorGUI.LabelField(position, new GUIContent(Space), new GUIContent(Space + currentValue));
			GUI.contentColor = cachedColor;
		}

		public static int GetSelectedIndex<T>(T value, Dictionary<string, T> map, GUIContent[] content)
		{
			string key = null;

			foreach (var pair in map)
			{
				if (pair.Value.Equals(value))
				{
					key = pair.Key;
					break;
				}
			}

			if (string.IsNullOrEmpty(key)) return -1;

			for (int i = 0; i < content.Length; i++)
			{
				if (content[i].text == key)
					return i;
			}

			return -1;
		}
	}
}