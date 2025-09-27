using System;
using System.Collections.Generic;

namespace DTech.ConstantDropdown.Editor
{
	public abstract class ConstMapCollectorBase<T> : IConstMapCollector<T>
	{
		public abstract int Priority { get; }
		
		public void Collect(Dictionary<Type, Dictionary<string, T>> map)
		{
			Dictionary<Type, Dictionary<string, T>> results = CollectInternal();
			foreach (var result in results)
			{
				map.TryAdd(result.Key, result.Value);
			}
		}
		
		protected abstract Dictionary<Type, Dictionary<string, T>> CollectInternal();
	}
}