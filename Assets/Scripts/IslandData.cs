using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class IslandData : MonoBehaviour
{
	public Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();
	public Dictionary<Vector2, MapObject> mapObjects = new Dictionary<Vector2, MapObject>();
	public int mapGridRadius;
	public int tileCount;
	public float heightMin;
	public float heightMax;
	public int heightSeeds;
	public float roundness;
	public bool drawConnections;

	public static IslandData Instance;
	void Awake() { Instance = this; }

	void Start()
	{
		AddTiles();
		FindTileHeights();
		FindConnections();
		GetComponent<IslandMesh>().GenMesh();
	}

	void AddTiles()
	{
		if (mapGridRadius > 0)
		{
			tiles.Add(Vector2.zero, new Tile(Vector2.zero));
			for (int fRadius = 1; fRadius <= mapGridRadius; fRadius++)
			{
				//Set initial hex grid location
				Vector2 gridLoc = new Vector2(fRadius, -fRadius);

				int dir = 2;
				//Find data for each hex in the ring (each ring has 6 more hexes than the last)
				for (int fHex = 0; fHex < 6 * fRadius; fHex++)
				{
					tiles.Add(gridLoc, new Tile(gridLoc));
					//Finds next hex in ring
					gridLoc = Grid.Instance.MoveTo(gridLoc, dir);
					if (gridLoc.x == 0 || gridLoc.y == 0 || gridLoc.x == -gridLoc.y)
					{
						dir++;
					}
				}
			}
		}
		else
		{
			tiles.Add(Vector2.zero, new Tile(Vector2.zero));
			List<Vector2> possibleAdds = new List<Vector2>(Grid.Instance.FindAdjacentGridLocs(Vector2.zero));
			while(tiles.Count < tileCount)
			{
				Vector2 gridLoc = possibleAdds[Mathf.RoundToInt(Random.Range(0, possibleAdds.Count) / roundness)];
				if (!tiles.ContainsKey(gridLoc))
				{
					tiles.Add(gridLoc, new Tile(gridLoc));
					foreach(Vector2 adj in Grid.Instance.FindAdjacentGridLocs(gridLoc))
					{
						if(!possibleAdds.Contains(adj) && !tiles.ContainsKey(adj))
						{
							possibleAdds.Add(adj);
						}
					}
					possibleAdds.Remove(gridLoc);
				}
			}
		}
	}

	void FindConnections()
	{
		foreach (Tile tile in tiles.Values)
		{
			foreach(Vector2 gridLoc in Grid.Instance.FindAdjacentGridLocs(tile.gridLoc))
			{
				if (tiles.ContainsKey(gridLoc) && Mathf.Abs(tile.height - tiles[gridLoc].height) <= Grid.Instance.maxHeightDifference)
				{
					tile.connections.Add(gridLoc);
					//if(drawConnections)
						//Debug.DrawLine(tile.worldLoc, tiles[gridLoc].worldLoc, Color.red, Mathf.Infinity);
				}
			}
		}
	}

	void FindTileHeights()
	{
		foreach (Tile tile in tiles.Values)
		{
			tile.SetHeight(Random.Range(0f, heightMax));
		}
	}
}
