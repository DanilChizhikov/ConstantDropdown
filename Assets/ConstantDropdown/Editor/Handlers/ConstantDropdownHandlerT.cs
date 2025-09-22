using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DTech.ConstantDropdown.Editor
{
	public abstract class ConstantDropdownHandlerT<T> : IConstantDropdownHandler
	{
		private readonly ConstMap<T> _map = new();
		private readonly ConstantSearchProviderT<T> _provider = new();
		
		public abstract SerializedPropertyType ServicedPropertyType { get; }

		public void RefreshMap() => _map.Clear();
		
		public GUIContent GetCurrentValue(SerializedProperty property)
		{
			T value = GetPropertyValue(property);
			return new GUIContent(value.ToString());
		}

		public bool TryGetProvider(SerializedProperty property, ConstantDropdownAttribute attribute, out ConstantSearchProvider provider)
		{
			provider = _provider;
			if (!_map.TryGetSourceType(attribute.LinkingType, out Dictionary<string, T> map, out GUIContent[] content))
			{
				return false;
			}

			_provider.Setup(map, (_, value) => SetPropertyValue(property, value));
			return true;
		}

		protected abstract T GetPropertyValue(SerializedProperty property);

		protected abstract void SetPropertyValue(SerializedProperty property, T value);
	}
}