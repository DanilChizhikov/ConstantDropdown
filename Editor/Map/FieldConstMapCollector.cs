using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace DTech.ConstantDropdown.Editor
{
	internal sealed class FieldConstMapCollector<T> : ConstMapCollectorBase<T>
	{
		public override int Priority => 1;

		protected override Dictionary<Type, Dictionary<string, T>> CollectInternal()
		{
			var result = new Dictionary<Type, Dictionary<string, T>>();
			var collection = TypeCache.GetFieldsWithAttribute<ConstantSourceAttribute>();

			foreach (FieldInfo field in collection)
			{
				var attribute = field.GetCustomAttribute<ConstantSourceAttribute>();
				if (attribute == null) continue;

				if (field.GetValue(null) is ICollection<T> values &&
					values.Count > 0)
				{
					Dictionary<string, T> sourceMap = values.ToDictionary(x => x.ToString(), x => x);
					result[attribute.LinkingType] = sourceMap;
				}
			}

			return result;
		}
	}
}