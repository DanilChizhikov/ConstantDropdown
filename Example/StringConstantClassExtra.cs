namespace DTech.ConstantDropdown.Example
{
	[ConstantSource(typeof(StringConstantClass))]
	internal static class StringConstantClassExtra
	{
		public const string ConstFirst = "ExtraFirst";
		public const string ConstThird = "Third";
	}
}