using UnityEditor;

namespace DTech.ConstantDropdown.Editor
{
	public interface IConstantDropdownHandler
	{
		SerializedPropertyType ServicedPropertyType { get; }

		void RefreshMap();
		bool Draw(ConstantDrawInfo info);
	}
}