using System;
using System.Collections.Generic;

namespace DTech.ConstantDropdown.Editor
{
	internal sealed class ConstMapCollection<T>
	{
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
			
			for (int i = 0; i < _collectors.Count; i++)
			{
				IConstMapCollector<T> collector = _collectors[i];
				collector.Collect(_map);
			}

			_isMapColleted = true;
		}
	}
}