using System;
using UnityEngine;

namespace DTech.ConstantDropdown
{
    public sealed class ConstantDropdownAttribute : PropertyAttribute
    {
        public Type LinkingType { get; }

        public ConstantDropdownAttribute(Type linkingType)
        {
            LinkingType = linkingType;
        }
    }
}
