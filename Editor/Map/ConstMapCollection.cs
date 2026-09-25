using System;
using System.Collections.Generic;
using UnityEngine;

namespace DTech.ConstantDropdown.Editor
{
	internal sealed class ConstMapCollection<T>
	{
		private const string DuplicateKeyWarningTemplate = "[ConstantDropdown] Duplicate key [{0}] for linked type [{1}], skipped";
		
		private static readonly List<IConstMapCollector<T>> _collectors = new();
		
		private readonly Dictionary<Type, Dictionary<string, T>> _map;
		
		private bool _isMapColleted;

		static ConstMapCollection()
		{
			RegisterCollector(new TypeConstMapCollector<T>());
			RegisterCollector(new FieldConstMapCollector<T>());
		}

		public ConstMapCollection()
		{
			_map = new Dictionary<Type, Dictionary<string, T>>();
			_isMapColleted = false;
		}
		
		public static void RegisterCollector(IConstMapCollector<T> collector)
		{
			if (collector == null)
			{
				throw new ArgumentNullException(nameof(collector));
			}

			_collectors.Add(collector);
			_collectors.Sort((a, b) => a.Priority.CompareTo(b.Priority));
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
			
			var sources = new Dictionary<Type, List<ConstSource<T>>>();
			for (int i = 0; i < _collectors.Count; i++)
			{
				IConstMapCollector<T> collector = _collectors[i];
				collector.Collect(sources);
			}

			foreach (var (linkedType, typeSources) in sources)
			{
				_map[linkedType] = MergeSources(linkedType, typeSources);
			}

			_isMapColleted = true;
		}

		private static Dictionary<string, T> MergeSources(Type linkedType, List<ConstSource<T>> sources)
		{
			if (sources.Count == 1)
			{
				return sources[0].Values;
			}

			var merged = new Dictionary<string, T>();
			foreach (ConstSource<T> source in sources)
			{
				foreach (var (key, value) in source.Values)
				{
					string mergedKey = $"{source.Name}/{key}";
					if (!merged.TryAdd(mergedKey, value))
					{
						Debug.LogWarning(string.Format(DuplicateKeyWarningTemplate, mergedKey, linkedType));
					}
				}
			}

			return merged;
		}
	}
}