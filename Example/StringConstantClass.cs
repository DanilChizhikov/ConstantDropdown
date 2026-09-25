namespace DTech.ConstantDropdown.Example
{
	[ConstantSource(typeof(StringConstantClass))]
	internal static class StringConstantClass
	{
		public const string ConstFirst = "First";
		public const string ConstSecond = "Second";
	}
}