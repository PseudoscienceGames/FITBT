using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack : PawnAction
{
	public int range;
	public int damage;

	void Start()
	{
		actionName = "Attack";
		cost = 4;
	}

	public override void SetUp()
	{
		base.SetUp();
		Pawn pawn = GetComponent<Pawn>();
		possibleMoves.Clear();
		List<Vector2> toCheck = new List<Vector2>();
		List<Vector2> checkNext = new List<Vector2>();
		checkNext.Add(pawn.gridLoc);

		for (int i = 1; i <= range; i++)
		{
			toCheck.Clear();
			toCheck = new List<Vector2>(checkNext);
			checkNext.Clear();
			foreach (Vector2 gridLoc in toCheck)
			{
				List<Vector2> adjacentTiles = Grid.FindAdjacentGridLocs(gridLoc);
				foreach (Vector2 adjGridLoc in adjacentTiles)
				{
					if (!possibleMoves.ContainsKey(adjGridLoc) && IslandData.Instance.tiles[gridLoc].connections.Contains(adjGridLoc) && IslandData.Instance.mapObjects.ContainsKey(adjGridLoc))
					{
						possibleMoves.Add(adjGridLoc, gridLoc);
						checkNext.Add(adjGridLoc);
					}
				}
			}
		}
		//foreach (Vector2 loc in possibleMoves.Keys)
		//	Debug.DrawLine(Grid.GridToWorld(pawn.gridLoc, 0), Grid.GridToWorld(loc, 0), Color.red, Mathf.Infinity);
	}

	public override void Act(Vector2 gridLoc)
	{
		NotificationCenter.DefaultCenter.PostNotification(null, "Moving");
		StopAllCoroutines();
		StartCoroutine(Move(gridLoc));
		base.Act(gridLoc);
	}

	IEnumerator Move(Vector2 gridLoc)
	{
		Pawn pawn = GetComponent<Pawn>();
		float timer = 0;
		while(timer < 1)
		{
			timer += Time.deltaTime;
			transform.Rotate(Vector3.up * timer * 360f);
			yield return null;
		}
		pawn.ap -= cost;
		IslandData.Instance.mapObjects[gridLoc].GetComponent<Pawn>().TakeDamage(damage);
		NotificationCenter.DefaultCenter.PostNotification(null, "MoveDone");
		if (GetComponent<AIControl>() != null)
			GetComponent<AIControl>().Activate();
	}
}
