using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DTech.ConstantDropdown.Editor
{
    [CustomPropertyDrawer(typeof(ConstantDropdownAttribute))]
    internal sealed class ConstantDropdownDrawer : PropertyDrawer
    {
        private const float ResetButtonWidth = 50f;
        
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

            Rect positionAfterLabel = EditorGUI.PrefixLabel(position, label);
            var dropdownRect = new Rect(positionAfterLabel.x, positionAfterLabel.y,
                positionAfterLabel.width - ResetButtonWidth - 2f, positionAfterLabel.height);
            if (GUI.Button(dropdownRect, handler.GetCurrentValue(property), EditorStyles.popup))
            {
                var context = new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition));
                if (handler.TryGetProvider(property, dropdownAttribute, out ConstantSearchProvider provider))
                {
                    SearchWindow.Open(context, provider);
                }
                else
                {
                    Debug.LogError("This property type is not supported.");
                }
            }
            
            var resetButtonRect = new Rect(dropdownRect.xMax + 2f, positionAfterLabel.y, ResetButtonWidth, positionAfterLabel.height);
            if (GUI.Button(resetButtonRect, "Reset"))
            {
                handler.RefreshMap();
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
                if (!type.IsClass || type.IsAbstract || type.IsInterface)
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