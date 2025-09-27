using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace DTech.ConstantDropdown.Editor
{
	internal sealed class ConstMapCollection<T>
	{
		private const string InvalidCastExceptionTemplate = "Value is [{0}] but need [{1}]";
		
		private readonly Dictionary<Type, Dictionary<string, T>> _map;

		private bool _isMapColleted;

		public ConstMapCollection()
		{
			_map = new Dictionary<Type, Dictionary<string, T>>();
			_isMapColleted = false;
		}

		public bool TryGetMap(Type linkedType, out Dictionary<string, T> map)
		{
			CollectMap();
			map = null;
			if (_map.TryGetValue(linkedType, out map))
			{
				return true;
			}

			return false;
		}

		public void Clear()
		{
			_map.Clear();
			_isMapColleted = false;
		}

		private void CollectMap()
		{
			if (_isMapColleted)
			{
				return;
			}
			
			CollectByTypes();
			CollectByFields();

			_isMapColleted = true;
		}

		private void CollectByTypes()
		{
			TypeCache.TypeCollection collection = TypeCache.GetTypesWithAttribute<ConstantSourceAttribute>();
			foreach (Type type in collection)
			{
				var attribute = type.GetCustomAttribute<ConstantSourceAttribute>();
				if (attribute == null)
				{
					continue;
				}
				
				if (!_map.ContainsKey(attribute.LinkingType))
				{
					Dictionary<string, T> sourceMap = GetSourceMap(type);
					if (sourceMap.Count <= 0)
					{
						continue;
					}
                            
					_map.Add(attribute.LinkingType, sourceMap);
					break;
				}
			}
		}

		private void CollectByFields()
		{
			TypeCache.FieldInfoCollection collection = TypeCache.GetFieldsWithAttribute<ConstantSourceAttribute>();
			foreach (FieldInfo field in collection)
			{
				var attribute = field.GetCustomAttribute<ConstantSourceAttribute>();
				if (attribute == null)
				{
					continue;
				}

				ICollection<T> value = field.GetValue(null) as ICollection<T>;
				if (value == null)
				{
					continue;
				}
				
				if (!_map.ContainsKey(attribute.LinkingType))
				{
					var sourceMap = new Dictionary<string, T>();
					foreach (T item in value)
					{
						sourceMap.Add(item.ToString(), item);
					}

					if (sourceMap.Count <= 0)
					{
						continue;
					}
					
					_map.Add(attribute.LinkingType, sourceMap);
					break;
				}
			}
		}
		
		private static Dictionary<string, T> GetSourceMap(Type type)
		{
			var dictionary = new Dictionary<string, T>();
			FieldInfo[] fieldInfos = type.GetFields();
			GetSourceMap(fieldInfos, true, dictionary);
			
			return dictionary;
		}

		private static void GetSourceMap(FieldInfo[] fieldInfos, bool isStatic, Dictionary<string, T> dictionary)
		{
			dictionary.Clear();
			foreach (FieldInfo field in fieldInfos)
			{
				if (field.FieldType != typeof(T) || (isStatic && !field.IsStatic))
				{
					continue;
				}

				object value = field.GetValue(null);
				if (value is not T genericValue)
				{
					string message = string.Format(InvalidCastExceptionTemplate, value.GetType(), typeof(T));
					throw new InvalidCastException(message);
				}
				
				dictionary.Add(field.Name, genericValue);
			}
		}
	}
}