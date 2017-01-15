using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndTurn : PawnAction
{
	void Start()
	{
		actionName = "End Turn";
		cost = 0;
	}

	public override void SetUp()
	{
		base.SetUp();
		//Debug.Log(transform.name + " " + "EndTurn");
		GetComponent<Pawn>().EndTurn();
	}

	public override void SetButton(GameObject button)
	{
		base.SetButton(button);
		button.GetComponent<Button>().onClick.AddListener(delegate { Cursor.Instance.SetAction(); });
	}
}
