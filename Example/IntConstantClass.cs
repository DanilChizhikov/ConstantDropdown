namespace DTech.ConstantDropdown.Example
{
	[ConstantSource(typeof(IntConstantClass))]
	internal static class IntConstantClass
	{
		public const int ConstOne = 1;
		public const int ConstTwo = 2;
	}
}