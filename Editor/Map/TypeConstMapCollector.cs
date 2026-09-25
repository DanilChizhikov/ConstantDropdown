using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace DTech.ConstantDropdown.Editor
{
	internal sealed class TypeConstMapCollector<T> : ConstMapCollectorBase<T>
	{
		private const string InvalidCastExceptionTemplate = "Value is [{0}] but need [{1}]";
		
		public override int Priority => 0;
		
		protected override Dictionary<Type, Dictionary<string, T>> CollectInternal()
		{
			var result = new Dictionary<Type, Dictionary<string, T>>();
			var collection = TypeCache.GetTypesWithAttribute<ConstantSourceAttribute>();
            
			foreach (Type type in collection)
			{
				var attribute = type.GetCustomAttribute<ConstantSourceAttribute>();
				if (attribute == null) continue;

				Dictionary<string, T> sourceMap = GetSourceMap(type);
				if (sourceMap.Count > 0)
				{
					result[attribute.LinkingType] = sourceMap;
				}
			}
            
			return result;
		}
		
		private static Dictionary<string, T> GetSourceMap(Type type)
		{
			var dictionary = new Dictionary<string, T>();
			FieldInfo[] fieldInfos = type.GetFields();
			foreach (FieldInfo field in fieldInfos)
			{
				if (field.FieldType != typeof(T) || !field.IsStatic)
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
			
			return dictionary;
		}
	}
}