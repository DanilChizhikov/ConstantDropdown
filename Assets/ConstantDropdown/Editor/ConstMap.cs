using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DTech.ConstantDropdown.Editor
{
	internal sealed class ConstMap<T>
	{
		private readonly Dictionary<Type, Dictionary<string, T>> _map;
		private readonly Dictionary<Type, GUIContent[]> _contents;

		private bool _isInitialized;

		public ConstMap()
		{
			_map = new Dictionary<Type, Dictionary<string, T>>();
			_contents = new Dictionary<Type, GUIContent[]>();
			_isInitialized = false;
		}

		public bool TryGetSourceType(Type linkedType, out Dictionary<string, T> map, out GUIContent[] content)
		{
			Initialize();
			map = null;
			content = null;
			if (_map.TryGetValue(linkedType, out map))
			{
				content = _contents[linkedType];
				return true;
			}

			return false;
		}

		public void Clear()
		{
			_map.Clear();
			_contents.Clear();
			_isInitialized = false;
		}

		private void Initialize()
		{
			if (_isInitialized)
			{
				return;
			}
			
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (Type type in assembly.GetTypes())
				{
					if (!type.IsClass)
					{
						continue;
					}

					IEnumerable<Attribute> attributes = type.GetCustomAttributes();
					foreach (Attribute attribute in attributes)
					{
						if (attribute is ConstantSourceAttribute sourceAttribute &&
							!_map.ContainsKey(sourceAttribute.LinkingType))
						{
							Dictionary<string,T> sourceMap = GetSourceMap(type);
							if (sourceMap.Count <= 0)
							{
								continue;
							}
                            
							_map.Add(sourceAttribute.LinkingType, sourceMap);
							GUIContent[] content = GetContents(sourceMap);
							_contents.Add(sourceAttribute.LinkingType, content);
							break;
						}
					}
				}
			}

			_isInitialized = true;
		}
		
		private static Dictionary<string, T> GetSourceMap(Type type)
		{
			var dictionary = new Dictionary<string, T>();
			FieldInfo[] fieldInfos = type.GetFields();
			foreach (FieldInfo field in fieldInfos)
			{
				if (field.FieldType != typeof(T) ||
					!field.IsStatic)
				{
					continue;
				}

				object value = field.GetValue(null);
				if (value is not T genericValue)
				{
					throw new InvalidCastException($"value is [{value.GetType()}] but need [{typeof(T)}]");
				}
				
				dictionary.Add(field.Name, genericValue);
			}

			return dictionary;
		}

		private static GUIContent[] GetContents(Dictionary<string, T> sourceMap)
		{
			var result = new List<GUIContent>();
			if (sourceMap.Count > 0)
			{
				foreach (string key in sourceMap.Keys)
				{
					result.Add(new GUIContent(key));
				}
			}

			return result.ToArray();
		}
	}

	internal sealed class IntMap
	{
		private readonly Dictionary<Type, Dictionary<string, int>> _map;
		private readonly Dictionary<Type, GUIContent[]> _contents;

		public IntMap()
		{
			_map = new Dictionary<Type, Dictionary<string, int>>();
			_contents = new Dictionary<Type, GUIContent[]>();
		}
	}
}