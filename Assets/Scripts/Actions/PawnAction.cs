using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PawnAction : MonoBehaviour
{
	public string actionName;
	public float cost;
	public bool isMove = false;
	public Dictionary<Vector2, Vector2> possibleMoves = new Dictionary<Vector2, Vector2>();

	public void SetButton(GameObject button)
	{
		//Debug.Log("SetButton" + actionName);
		button.transform.GetChild(0).GetComponent<Text>().text = actionName;
		button.GetComponent<Button>().onClick.AddListener(delegate { Cursor.Instance.SelectAction(this); });
	}

	public virtual void SetUp()
	{
		//Debug.Log("SetUp" + actionName);
	}

	public virtual void Act(Vector2 gridLoc)
	{
		//Debug.Log("Act" + actionName);
	}
}
