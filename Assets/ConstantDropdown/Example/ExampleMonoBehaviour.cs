using UnityEngine;

namespace DTech.ConstantDropdown.Example
{
	internal sealed class ExampleMonoBehaviour : MonoBehaviour
	{
		[Header("Strings")]
		[SerializeField, ConstantDropdown(typeof(StringConstantClass))]
		private string _stringConst;
		
		[SerializeField, StringDropdown]
		private string _concreteStringConst;
		
		[SerializeField, ConstantDropdown(typeof(StingCollectionClass), "Collection")]
		private string _collectionStringItem;

		[Header("Integers")]
		[SerializeField, ConstantDropdown(typeof(IntConstantClass))]
		private int _intConst;
		
		[Header("Floats")]
		[SerializeField, ConstantDropdown(typeof(FloatConstantClass))]
		private float _floatConst;

		[ContextMenu(nameof(DrawFieldValues))]
		private void DrawFieldValues()
		{
			Debug.Log($"{nameof(_stringConst)} is {_stringConst}");
			Debug.Log($"{nameof(_concreteStringConst)} is {_concreteStringConst}");
			Debug.Log($"{nameof(_collectionStringItem)} is {_collectionStringItem}");
			Debug.Log($"{nameof(_intConst)} is {_intConst}");
			Debug.Log($"{nameof(_floatConst)} is {_floatConst}");
		}
	}
}