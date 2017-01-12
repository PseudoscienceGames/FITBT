using UnityEngine;
using System.Collections;

public class PawnAction : MonoBehaviour
{
	public string actionName;
	public float cost;
	public bool isMove = false;

	public virtual void SetUp()
	{

	}

	public virtual void Act(Vector2 gridLoc)
	{
		//if (isMove)
		//{
		//	GetComponent<Unit>().ap -= cost * Vector3.Distance(GetComponent<MapObject>().gridLoc.ToVector3(), gridLoc.ToVector3());
		//}
		//else
		//{
		//	GetComponent<Unit>().ap -= cost;
		//}
		//TurnController.Instance.NextUnit();
	}
}
