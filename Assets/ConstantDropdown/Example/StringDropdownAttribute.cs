using System;

namespace DTech.ConstantDropdown.Example
{
    internal sealed class StringDropdownAttribute : ConstantDropdownBaseAttribute
    {
        public override Type LinkingType => typeof(StringConstantClass);
    }
}