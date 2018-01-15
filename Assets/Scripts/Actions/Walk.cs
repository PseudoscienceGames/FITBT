using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Walk : PawnAction
{
	public float moveSpeed;//tiles per ap

	void Start()
	{
		actionName = "Move";
		cost = 1;
		isMove = true;
	}

	public override void SetUp()
	{
		base.SetUp();
		Pawn pawn = GetComponent<Pawn>();
		possibleMoves.Clear();
		List<Vector2> toCheck = new List<Vector2>();
		List<Vector2> checkNext = new List<Vector2>();
		float maxMove = pawn.ap * moveSpeed;
		checkNext.Add(pawn.gridLoc);

        for (int i = 1; i <= maxMove; i++)
		{
			toCheck.Clear();
			toCheck = new List<Vector2>(checkNext);
			checkNext.Clear();
			foreach (Vector2 gridLoc in toCheck)
			{
				List<Vector2> adjacentTiles = Grid.FindAdjacentGridLocs(gridLoc);
				foreach(Vector2 adjGridLoc in adjacentTiles)
				{
					if (!possibleMoves.ContainsKey(adjGridLoc) && IslandData.Instance.tiles[gridLoc].connections.Contains(adjGridLoc) && !IslandData.Instance.mapObjects.ContainsKey(adjGridLoc))
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
		List<Vector2> moves = new List<Vector2>();
		Vector2 next = gridLoc;
		while(next != pawn.gridLoc)
		{
			moves.Add(next);
			next = possibleMoves[next];
		}
		moves.Reverse();
		pawn.UseAP(moves.Count * cost);
		foreach (Vector2 move in moves)
		{
			//Debug.DrawLine(transform.position, Grid.GridToWorld(move, IslandData.Instance.tiles[move].height), Color.green, Mathf.Infinity);
			Vector3 worldLoc = Grid.GridToWorld(move, IslandData.Instance.tiles[move].height);
			Vector3 initialPos = transform.position;
			float timer = 0;
			int x = 0;
			transform.LookAt(Grid.GridToWorld(move, transform.position.y));
			while (Vector3.Distance(transform.position, worldLoc) > 0 && x < 1000)
			{
				x++;
				transform.position = Vector3.Lerp(initialPos, worldLoc, timer * 2);
				Camera.main.transform.root.position = new Vector3(transform.position.x, 0, transform.position.z);
				Selector.Instance.transform.position = transform.position;
				timer += Time.deltaTime;
				yield return null;
			}
		}
		IslandData.Instance.mapObjects.Remove(pawn.gridLoc);
        pawn.gridLoc = gridLoc;
		IslandData.Instance.mapObjects.Add(gridLoc, pawn);
		NotificationCenter.DefaultCenter.PostNotification(null, "MoveDone");
		if (GetComponent<AIControl>() != null)
			GetComponent<AIControl>().Activate();
	}
}
