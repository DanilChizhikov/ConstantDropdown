using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DTech.ConstantDropdown.Editor
{
    [CustomPropertyDrawer(typeof(ConstantDropdownBaseAttribute), true)]
    internal sealed class ConstantDropdownDrawer : PropertyDrawer
    {
        private const float MinButtonWidth = 50f;
        private const float ButtonPadding = 10f;
        private const float MinDropdownWidth = 100f;

        private static readonly Dictionary<SerializedPropertyType, IConstantDropdownHandler> _handlers = new();
        private static readonly GUIContent _resetButtonContent = new("Reset Const Map");
        
        private ConstantDropdownBaseAttribute Attribute => (ConstantDropdownBaseAttribute)attribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!(attribute is ConstantDropdownBaseAttribute))
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
            CalculateRects(positionAfterLabel, out Rect dropdownRect, out Rect buttonRect);
            DrawDropdown(dropdownRect, property, Attribute.LinkingType, handler);
            DrawResetButton(buttonRect, handler);
        }
        
        private void DrawDropdown(Rect position, SerializedProperty property, Type linkingType, IConstantDropdownHandler handler)
        {
            GUIContent dropdownCaption = handler.GetDropdownCaption(linkingType, property, Attribute.PrefixName);
            if (GUI.Button(position, dropdownCaption, EditorStyles.popup))
            {
                if (!handler.TrySelectValue(property, linkingType))
                {
                    Debug.LogError("This property type is not supported.");
                }
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

        private static void CalculateRects(Rect position, out Rect dropdownRect, out Rect buttonRect)
        {
            float buttonWidth = Mathf.Max(MinButtonWidth, EditorStyles.miniButton.CalcSize(_resetButtonContent).x + ButtonPadding);
            
            dropdownRect = new Rect(position.x, position.y, position.width - buttonWidth - 2f, position.height);
            buttonRect = new Rect(dropdownRect.xMax + 2f, position.y, buttonWidth, position.height);
            
            if (dropdownRect.width < MinDropdownWidth)
            {
                dropdownRect.width = MinDropdownWidth;
                buttonRect.x = dropdownRect.xMax + 2f;
                if (buttonRect.xMax > position.xMax)
                {
                    dropdownRect.width = position.width;
                    buttonRect.x = position.x + position.width + 2f;
                }
            }
        }

        private static void DrawResetButton(Rect position, IConstantDropdownHandler handler)
        {
            if (GUI.Button(position, _resetButtonContent, EditorStyles.miniButton))
            {
                handler.RefreshMap();
            }
        }
    }
}