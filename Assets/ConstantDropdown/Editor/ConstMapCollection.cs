using System;
using System.Collections.Generic;
using System.Reflection;

namespace DTech.ConstantDropdown.Editor
{
	internal sealed class ConstMapCollection<T>
	{
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
							break;
						}
					}
				}
			}

			_isMapColleted = true;
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
	}
}