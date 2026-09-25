using UnityEngine;

namespace DTech.ConstantDropdown.Example
{
    [ConstantSource(typeof(FloatConstantClass))]
    internal static class FloatConstantClass
    {
        public const float ConstFirst = 1f;
        public const float ConstSecond = 2f;
        public const float Pi = Mathf.PI;
    }
}