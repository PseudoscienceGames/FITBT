using UnityEngine;
using System.Collections;

public class Walk : PawnAction
{
	void Start()
	{
		actionName = "Move";
		isMove = true;
	}

	public override void Act(Vector2 gridLoc)
	{
		StartCoroutine(Move(gridLoc));
		base.Act(gridLoc);
	}

	IEnumerator Move(Vector2 gridLoc)
	{
		Vector3 worldLoc = Grid.Instance.GridToWorld(gridLoc, IslandData.Instance.tiles[gridLoc].height);
		Vector3 initialPos = transform.position;
		float timer = 0;
		while (Vector3.Distance(transform.position, worldLoc) > 0)
		{
			transform.position = Vector3.Lerp(initialPos, worldLoc, timer);
			timer += Time.deltaTime;
			yield return null;
		}
		yield return null;
	}
}
