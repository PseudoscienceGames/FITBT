using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PawnAction : MonoBehaviour
{
	public string actionName;
	public float cost;
	public bool isMove = false;
	public Dictionary<Vector2, Vector2> possibleMoves = new Dictionary<Vector2, Vector2>();

	public virtual void SetUp()
	{

	}

	public virtual void Act(Vector2 gridLoc)
	{
		//if (isMove)
		//{
		//	GetComponent<Pawn>().ap -= cost * Vector3.Distance(GetComponent<MapObject>().gridLoc.ToVector3(), gridLoc.ToVector3());
		//}
		//else
		//{
		//	GetComponent<Unit>().ap -= cost;
		//}
		TurnController.Instance.NextUnit();
	}
}
