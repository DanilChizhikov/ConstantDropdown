using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace DTech.ConstantDropdown.Editor
{
	internal sealed class FieldConstMapCollector<T> : ConstMapCollectorBase<T>
	{
		public override int Priority => 1;

		protected override List<(Type LinkingType, ConstSource<T> Source)> CollectInternal()
		{
			var result = new List<(Type LinkingType, ConstSource<T> Source)>();
			var collection = TypeCache.GetFieldsWithAttribute<ConstantSourceAttribute>();

			foreach (FieldInfo field in collection)
			{
				var attribute = field.GetCustomAttribute<ConstantSourceAttribute>();
				if (attribute == null) continue;

				if (field.GetValue(null) is ICollection<T> values &&
					values.Count > 0)
				{
					var sourceMap = new Dictionary<string, T>(values.Count);
					foreach (T value in values)
					{
						sourceMap.TryAdd(value.ToString(), value);
					}

					string sourceName = $"{field.DeclaringType?.Name}.{field.Name}";
					result.Add((attribute.LinkingType, new ConstSource<T>(sourceName, sourceMap)));
				}
			}

			return result;
		}
	}
}