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

            if (typeof(T) == typeof(string))
            {
                var tree = new Dictionary<string, object>();
                foreach (var kvp in _constants)
                {
                    string[] parts = kvp.Key.Split('/');
                    Dictionary<string, object> currentNode = tree;
                    for (int i = 0; i < parts.Length - 1; i++)
                    {
                        var part = parts[i];
                        if (!currentNode.ContainsKey(part))
                        {
                            currentNode[part] = new Dictionary<string, object>();
                        }
                        currentNode = (Dictionary<string, object>)currentNode[part];
                    }
                    
                    var lastPart = parts[^1];
                    currentNode[lastPart] = kvp;
                }
                
                AddTreeItems(entries, tree, 1);
            }
            else
            {
                AddNonStringEntries(entries);
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
        
        private void AddTreeItems(List<SearchTreeEntry> entries, Dictionary<string, object> node, int level)
        {
            foreach (var kvp in node)
            {
                if (kvp.Value is Dictionary<string, object> childNode)
                {
                    entries.Add(new SearchTreeGroupEntry(new GUIContent(kvp.Key), level));
                    AddTreeItems(entries, childNode, level + 1);
                }
                else if (kvp.Value is KeyValuePair<string, T> itemKvp)
                {
                    entries.Add(new SearchTreeEntry(new GUIContent(kvp.Key))
                    {
                        level = level,
                        userData = itemKvp
                    });
                }
            }
        }

        private void AddNonStringEntries(List<SearchTreeEntry> entries)
        {
            foreach (var kvp in _constants)
            {
                entries.Add(new SearchTreeEntry(new GUIContent(kvp.Key))
                {
                    level = 1,
                    userData = kvp
                });
            }
        }
    }
}