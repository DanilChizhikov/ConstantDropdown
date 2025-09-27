using System;

namespace DTech.ConstantDropdown
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public sealed class ConstantSourceAttribute : Attribute
	{
		public Type LinkingType { get; }

		public ConstantSourceAttribute(Type linkingType)
		{
			LinkingType = linkingType;
		}
	}
}