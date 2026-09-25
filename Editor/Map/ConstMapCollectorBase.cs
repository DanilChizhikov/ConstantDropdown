using System;
using System.Collections.Generic;

namespace DTech.ConstantDropdown.Editor
{
	public abstract class ConstMapCollectorBase<T> : IConstMapCollector<T>
	{
		public abstract int Priority { get; }
		
		public void Collect(Dictionary<Type, List<ConstSource<T>>> sources)
		{
			List<(Type LinkingType, ConstSource<T> Source)> results = CollectInternal();
			foreach (var (linkingType, source) in results)
			{
				if (!sources.TryGetValue(linkingType, out List<ConstSource<T>> list))
				{
					list = new List<ConstSource<T>>();
					sources.Add(linkingType, list);
				}
				
				list.Add(source);
			}
		}
		
		protected abstract List<(Type LinkingType, ConstSource<T> Source)> CollectInternal();
	}
}