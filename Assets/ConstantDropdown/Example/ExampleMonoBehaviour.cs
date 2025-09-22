using UnityEngine;

namespace DTech.ConstantDropdown.Example
{
	internal sealed class ExampleMonoBehaviour : MonoBehaviour
	{
		[SerializeField, ConstantDropdown(typeof(StringConstantClass))]
		private string _stringConst;

		[SerializeField, ConstantDropdown(typeof(IntConstantClass))]
		private int _intConst;

		[ContextMenu(nameof(DrawFieldValues))]
		private void DrawFieldValues()
		{
			Debug.Log($"{nameof(_stringConst)} is {_stringConst}");
			Debug.Log($"{nameof(_intConst)} is {_intConst}");
		}
	}
}