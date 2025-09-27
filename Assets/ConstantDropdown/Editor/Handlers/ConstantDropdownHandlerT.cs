using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DTech.ConstantDropdown.Editor
{
	public abstract class ConstantDropdownHandlerT<T> : IConstantDropdownHandler
	{
		private static readonly GUIContent _emptyContent = new(string.Empty);
		
		private readonly ConstMapCollection<T> _mapCollection = new();
		private readonly ConstantSearchProviderT<T> _provider = new();
		
		public abstract SerializedPropertyType ServicedPropertyType { get; }

		public void RefreshMap() => _mapCollection.Clear();
		
		public GUIContent GetDropdownCaption(Type linkedType, SerializedProperty property, string prefixName)
		{
			T value = GetPropertyValue(property);
			if (!_mapCollection.TryGetMap(linkedType, out Dictionary<string, T> map))
			{
				return _emptyContent;
			}

			string caption = GetKeyFromValue(value, map);
			if (!string.IsNullOrEmpty(prefixName))
			{
				caption = $"{prefixName}_{caption}";
			}
			
			return new GUIContent(caption);
		}

		public bool TrySelectValue(SerializedProperty property, Type linkedType)
		{
			if (!_mapCollection.TryGetMap(linkedType, out Dictionary<string, T> map))
			{
				return false;
			}
			
			_provider.Setup(map, (_, value) =>
			{
				SetPropertyValue(property, value);
				property.serializedObject.ApplyModifiedProperties();
			});
			
			var context = new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition));
			SearchWindow.Open(context, _provider);
			return true;
		}

		protected abstract T GetPropertyValue(SerializedProperty property);

		protected abstract void SetPropertyValue(SerializedProperty property, T value);
		
		private static string GetKeyFromValue(T value, Dictionary<string, T> map)
		{
			foreach (var entry in map)
			{
				if (entry.Value.Equals(value))
				{
					return entry.Key;
				}
			}
			
			return string.Empty;
		}
	}
}