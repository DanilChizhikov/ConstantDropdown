using System;
using UnityEngine;

namespace DTech.ConstantDropdown
{
    public abstract class ConstantDropdownBaseAttribute : PropertyAttribute
    {
        public abstract Type LinkingType { get; }
    }
}