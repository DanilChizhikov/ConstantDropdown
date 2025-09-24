using System;

namespace DTech.ConstantDropdown
{
	[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class ConstantSourceAttribute : Attribute
	{
		public Type LinkingType { get; }

		public ConstantSourceAttribute(Type linkingType)
		{
			LinkingType = linkingType;
		}
	}
}