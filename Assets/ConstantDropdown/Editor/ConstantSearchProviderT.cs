using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DTech.ConstantDropdown.Editor
{
	internal sealed class ConstantSearchProviderT<T> : ConstantSearchProvider
	{
		private Dictionary<string, T> _constants;
		private Action<string, T> _onSelectCallback;

		public void Setup(Dictionary<string, T> constants, Action<string, T> onSelectCallback)
		{
			_constants = constants;
			_onSelectCallback = onSelectCallback;
		}

		public override List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
		{
			var entries = new List<SearchTreeEntry>
			{
				new SearchTreeGroupEntry(new GUIContent(typeof(T).Name + " Constants"), 0)
			};

			foreach (var kvp in _constants)
			{
				entries.Add(new SearchTreeEntry(new GUIContent(kvp.Key))
				{
					level = 1,
					userData = kvp
				});
			}

			return entries;
		}

		public override bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
		{
			if (searchTreeEntry.userData is KeyValuePair<string, T> kvp)
			{
				_onSelectCallback?.Invoke(kvp.Key, kvp.Value);
				return true;
			}

			return false;
		}
	}
}