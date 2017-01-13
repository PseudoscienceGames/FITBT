using UnityEngine;
using System.Collections;

public class EndTurn : PawnAction
{
	void Start()
	{
		actionName = "End Turn";
	}

	public override void SetUp()
	{
		base.SetUp();
		//Debug.Log(transform.name + " " + "EndTurn");
		GetComponent<Pawn>().EndTurn();
	}
}
