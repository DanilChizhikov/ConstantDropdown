using System.Collections.Generic;

namespace DTech.ConstantDropdown.Editor
{
	public readonly struct ConstSource<T>
	{
		public string Name { get; }
		public Dictionary<string, T> Values { get; }

		public ConstSource(string name, Dictionary<string, T> values)
		{
			Name = name;
			Values = values;
		}
	}
}