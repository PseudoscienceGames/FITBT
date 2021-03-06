﻿using UnityEngine;
using System.Collections;

public class MapObject : MonoBehaviour
{
	public Vector2 gridLoc;

	public virtual void Start()
	{
		gridLoc = Grid.RoundToGrid(transform.position);
		transform.position = Grid.GridToWorld(gridLoc, IslandData.Instance.tiles[gridLoc].height);
		if (!IslandData.Instance.mapObjects.ContainsKey(gridLoc))
			IslandData.Instance.mapObjects.Add(gridLoc, this);
		else
			Debug.Log("GridLoc already occupied");
	}
}
