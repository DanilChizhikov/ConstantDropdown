namespace DTech.ConstantDropdown.Example
{
	internal static class StingCollectionClass
	{
		private const string Item1 = "Item_1";
		private const string Item2 = "Item_2";
		private const string Item3 = "Item_3/Item_1/Item_1";
		private const string Item4 = "Item_3/Item_1/Item_2";
		
		[ConstantSource(typeof(StingCollectionClass))]
		public static readonly string[] Items = new string[]
		{
			Item1,
			Item2,
			Item3,
			Item4,
		};
	}
}