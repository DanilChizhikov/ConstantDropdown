using System;
using System.Collections.Generic;

namespace DTech.ConstantDropdown.Editor
{
	public interface IConstMapCollector<T>
	{
		int Priority { get; }
		void Collect(Dictionary<Type, Dictionary<string, T>> map);
	}
}