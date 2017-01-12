using UnityEngine;
using System.Collections;

public class Pawn : MonoBehaviour
{
	public Vector2 gridLoc;
	public float totalAP;
	public float ap;

	void Start()
	{
		gridLoc = Grid.Instance.RoundToGrid(transform.position);
		transform.position = Grid.Instance.GridToWorld(gridLoc, IslandData.Instance.tiles[gridLoc].height);
		TurnController.Instance.units.Add(this);
	}

	public float GetInitiative()
	{
		return Random.Range(0f, 101f);
	}
}
