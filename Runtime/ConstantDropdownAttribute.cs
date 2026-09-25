using System;

namespace DTech.ConstantDropdown
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ConstantDropdownAttribute : ConstantDropdownBaseAttribute
    {
        public override Type LinkingType { get; }
        public override string PrefixName { get; }

        public ConstantDropdownAttribute(Type linkingType, string prefixName = "")
        {
            LinkingType = linkingType;
            PrefixName = prefixName;
        }
    }
}
