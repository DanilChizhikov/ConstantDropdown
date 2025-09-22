using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DTech.ConstantDropdown.Editor
{
    [CustomPropertyDrawer(typeof(ConstantDropdownAttribute))]
    internal sealed class ConstantDropdownDrawer : PropertyDrawer
    {
        private static readonly Dictionary<SerializedPropertyType, IConstantDropdownHandler> _handlers = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!(attribute is ConstantDropdownAttribute dropdownAttribute))
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            EnsureHandlers();
            if (!_handlers.TryGetValue(property.propertyType, out IConstantDropdownHandler handler))
            {
                EditorGUI.HelpBox(position, "This property type is not supported.", MessageType.Error);
                return;
            }
            
            var info = new ConstantDrawInfo(position, property, dropdownAttribute, label);
            if (!handler.Draw(info))
            {
                EditorGUI.HelpBox(position, $"Handler [{handler.GetType()}] cannot draw property.", MessageType.Error);
            }
        }

        private static void EnsureHandlers()
        {
            if (_handlers.Count > 0)
            {
                return;
            }

            TypeCache.TypeCollection collection = TypeCache.GetTypesDerivedFrom<IConstantDropdownHandler>();
            foreach (Type type in collection)
            {
                if (!type.IsClass ||
                    type.IsAbstract ||
                    type.IsInterface)
                {
                    continue;
                }
                
                if (Activator.CreateInstance(type) is IConstantDropdownHandler instance)
                {
                    _handlers.Add(instance.ServicedPropertyType, instance);
                }
            }
        }
    }
}