namespace DTech.ConstantDropdown.Example
{
	internal static class StingCollectionClass
	{
		[ConstantSource(typeof(StingCollectionClass))]
		private static readonly string[] _items = new string[]
		{
			"Item_1",
			"Item_2",
			"Item_3",
		};
	}
}