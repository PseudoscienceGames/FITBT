using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BotAI : AIControl
{
	public override void Activate()
	{
		//Debug.Log(transform.name + " Activate");
		Walk walk = GetComponent<Walk>();
		walk.SetUp();
		List<Vector2> possibleMoves = new List<Vector2>(walk.possibleMoves.Keys);
		if (possibleMoves.Count > 0)
			walk.Act(possibleMoves[Random.Range(0, possibleMoves.Count)]);
		else
			GetComponent<EndTurn>().SetUp();
	}
}
