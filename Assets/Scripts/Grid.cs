﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Grid
{
	public static float hexRadius = 0.5f;
	public static float maxHeightDifference = 0.25f;

	//Finds distance in number of hexes between the hex at grid location fromLoc and the hex at toLoc
	public static int FindGridDistance(Vector2 fromLoc, Vector2 toLoc)
	{
		int tempFromZ = (int)(0 - (fromLoc.x + fromLoc.y));
		int tempToZ = (int)(0 - (toLoc.x + toLoc.y));
		int distance = (int)(Mathf.Abs(fromLoc.x - toLoc.x) + Mathf.Abs(fromLoc.y - toLoc.y) + Mathf.Abs(tempFromZ - tempToZ)) / 2;
		return distance;
	}

	//Takes grid location and converts it to world location
	public static Vector3 GridToWorld(Vector2 gridLoc, float height)
	{
		int tempZ = (int)(0 - (gridLoc.x + gridLoc.y));
		Vector3 worldPos = new Vector3(0.5f * (gridLoc.y - tempZ) * Mathf.Sqrt(3) * hexRadius, height, 1.5f * gridLoc.x * hexRadius);
		return worldPos;
	}

	//Takes world location and converts it to grid location
	public static Vector2 RoundToGrid(Vector3 worldLoc)
	{
		Vector2 gridLoc;
		gridLoc.x = Mathf.Round(worldLoc.z / (1.5f * hexRadius));
		gridLoc.y = Mathf.Round((worldLoc.x / (Mathf.Sqrt(3) * hexRadius)) - (gridLoc.x * 0.5f));// - (gridLoc.x * hexRadius * Mathf.Sqrt(3) * 0.5f));
		return gridLoc;
	}

	//Fixes moveDir
	public static int MoveDirFix(int moveDir)
	{
		while (moveDir > 5)
			moveDir -= 6;
		while (moveDir < 0)
			moveDir += 6;
		return moveDir;
	}

	//Returns the grid location of the hex adjacent to the one at gridLoc in direction moveDir
	public static Vector2 MoveTo(Vector2 gridLoc, int moveDir)
	{
		moveDir = MoveDirFix(moveDir);
		Vector2 moveTo = gridLoc;
		if (moveDir == 0)
		{
			moveTo.x++;
			moveTo.y--;
		}
		if (moveDir == 1)
			moveTo.x++;
		if (moveDir == 2)
			moveTo.y++;
		if (moveDir == 3)
		{
			moveTo.x--;
			moveTo.y++;
		}
		if (moveDir == 4)
			moveTo.x--;
		if (moveDir == 5)
			moveTo.y--;
		return moveTo;
	}

	public static List<Vector2> FindAdjacentGridLocs(Vector2 gridLoc)
	{
		List<Vector2> adjacentLocs = new List<Vector2>();
		for (int i = 0; i < 6; i++)
			adjacentLocs.Add(MoveTo(gridLoc, i));
		return adjacentLocs;
	}
}
