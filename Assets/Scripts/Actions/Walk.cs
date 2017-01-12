using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Walk : PawnAction
{
	public float moveSpeed;//tiles per ap

	void Start()
	{
		actionName = "Move";
		isMove = true;
	}

	public override void SetUp()
	{
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
				List<Vector2> adjacentTiles = Grid.Instance.FindAdjacentGridLocs(gridLoc);
				foreach(Vector2 adjGridLoc in adjacentTiles)
				{
					if (!possibleMoves.ContainsKey(adjGridLoc) && IslandData.Instance.tiles[gridLoc].connections.Contains(adjGridLoc))
					{
						possibleMoves.Add(adjGridLoc, gridLoc);
						checkNext.Add(adjGridLoc);
					}
				}
			}
		}
		//foreach (Vector2 loc in possibleMoves.Keys)
		//	Debug.DrawLine(Grid.Instance.GridToWorld(pawn.gridLoc, 0), Grid.Instance.GridToWorld(loc, 0), Color.red, Mathf.Infinity);
    }

	public override void Act(Vector2 gridLoc)
	{
		StartCoroutine(Move(gridLoc));
		base.Act(gridLoc);
	}

	IEnumerator Move(Vector2 gridLoc)
	{
		List<Vector2> moves = new List<Vector2>();
		Vector2 next = gridLoc;
		while(next != GetComponent<Pawn>().gridLoc)
		{
			moves.Add(next);
			next = possibleMoves[next];
		}
		moves.Reverse();
		foreach (Vector2 move in moves)
		{
			Debug.DrawLine(transform.position, Grid.Instance.GridToWorld(move, IslandData.Instance.tiles[move].height), Color.green, Mathf.Infinity);
			Vector3 worldLoc = Grid.Instance.GridToWorld(move, IslandData.Instance.tiles[move].height);
			Vector3 initialPos = transform.position;
			float timer = 0;
			while (Vector3.Distance(transform.position, worldLoc) > 0)
			{
				transform.position = Vector3.Lerp(initialPos, worldLoc, timer * 5);
				timer += Time.deltaTime;
				yield return null;
			}
		}
		GetComponent<Pawn>().gridLoc = gridLoc;
		yield return null;
	}
}
