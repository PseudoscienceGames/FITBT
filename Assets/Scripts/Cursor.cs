using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Cursor : MonoBehaviour
{
	public PawnAction selectedAction;
	public bool on = false;
	public Vector2 gridLoc;
	public Vector2 oldGridLoc;
	public GameObject markerPrefab;

	public static Cursor Instance;
	void Awake() { Instance = this; }

	void Update()
	{
		if (on)
		{
			RaycastHit hit;
			oldGridLoc = gridLoc;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, LayerMask.GetMask("Ground")))
			{
				Vector2 tempGridLoc = Grid.RoundToGrid(hit.point);
				if (IslandData.Instance.tiles.ContainsKey(tempGridLoc) && (selectedAction.possibleMoves.ContainsKey(tempGridLoc) || selectedAction.possibleMoves.Count == 0))
				{
					gridLoc = tempGridLoc;
					transform.position = Grid.GridToWorld(gridLoc, IslandData.Instance.tiles[gridLoc].height);
					if (gridLoc != oldGridLoc && selectedAction.GetComponent<AIControl>() == null && selectedAction.actionName == "Move")
						ShowPath();
				}
			}
			if (Input.GetMouseButtonDown(0))
			{
				SetAction();
			}
		}
	}

	public void SelectAction(PawnAction action)
	{
		if (action.GetComponent<Pawn>().ap >= action.cost)
		{
			selectedAction = action;
			transform.GetChild(0).gameObject.SetActive(true);
			action.SetUp();
			on = true;
		}
	}

	public void SetAction()
	{
		ClearPath();
		selectedAction.Act(gridLoc);
		selectedAction = null;
		transform.GetChild(0).gameObject.SetActive(false);
		on = false;
	}

	void ShowPath()
	{
		ClearPath();
		Vector2 next = gridLoc;
		while (next != selectedAction.GetComponent<Pawn>().gridLoc)
		{
			GameObject marker = Instantiate(markerPrefab, Grid.GridToWorld(next, IslandData.Instance.tiles[next].height), Quaternion.identity) as GameObject;
			marker.transform.parent = transform;
			next = selectedAction.possibleMoves[next];
		}
	}

	void ClearPath()
	{
		for (int i = transform.childCount - 1; i > 0; i--)
		{
			if (transform.GetChild(i).name != "Selector")
				Destroy(transform.GetChild(i).gameObject);
		}
	}
}
