using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour
{
	public Pawn selection;

	public static Selector Instance;
	void Awake() { Instance = this; }

	public void SetSelection(Pawn unit)
	{
		selection = unit;
		transform.position = selection.transform.position;
	}
}
