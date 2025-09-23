using System;

namespace DTech.ConstantDropdown
{
    public sealed class ConstantDropdownAttribute : ConstantDropdownBaseAttribute
    {
        public override Type LinkingType { get; }

        public ConstantDropdownAttribute(Type linkingType)
        {
            LinkingType = linkingType;
        }
    }
}
