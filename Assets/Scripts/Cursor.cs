using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour
{
	public PawnAction selectedAction;
	public bool on = false;
	public Vector2 gridLoc;

	public static Cursor Instance;
	void Awake() { Instance = this; }

	void Update()
	{
		if (on)
		{
			RaycastHit hit;

			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, LayerMask.GetMask("Ground")))
			{
				Vector2 tempGridLoc = Grid.Instance.RoundToGrid(hit.point);
				if (IslandData.Instance.tiles.ContainsKey(tempGridLoc) && (selectedAction.possibleMoves.ContainsKey(tempGridLoc) || selectedAction.possibleMoves.Count == 0))
				{
					gridLoc = tempGridLoc;
					transform.position = Grid.Instance.GridToWorld(gridLoc, IslandData.Instance.tiles[gridLoc].height); ;
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
		selectedAction = action;
		transform.GetChild(0).gameObject.SetActive(true);
		action.SetUp();
		on = true;
	}

	void SetAction()
	{
		selectedAction.Act(gridLoc);
		selectedAction = null;
		transform.GetChild(0).gameObject.SetActive(false);
		on = false;
	}
}
