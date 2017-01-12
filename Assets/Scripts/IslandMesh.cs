using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IslandMesh : MonoBehaviour
{
	private List<Vector3> verts = new List<Vector3>();
	private List<int> tris = new List<int>();
	private List<Vector2> uvs = new List<Vector2>();
	private int vertNumber = 0;
	public bool addNoise;

	IslandData data;

	public void GenMesh()
	{
		data = GetComponent<IslandData>();

		AddTops();
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.vertices = verts.ToArray();
		mesh.triangles = tris.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.RecalculateNormals();
		GetComponent<MeshCollider>().sharedMesh = mesh;
		Debug.Log(verts.Count);
	}

	void AddTops()
	{
		foreach (Tile tile in GetComponent<IslandData>().tiles.Values)
		{
			AddTop(tile);
			AddSide(tile);
		}
	}
	void AddTop(Tile tile)
	{
		verts.Add(tile.worldLoc);
		for (int i = 0; i <= 5; i++)
		{
			Vector3 vertex1WorldLoc = tile.worldLoc + (Quaternion.Euler(0, (60 * i), 0) * Vector3.forward * Grid.Instance.hexRadius);
			float height1 = FindVertHeight(tile, i);
			vertex1WorldLoc.y = height1;
			verts.Add(AddNoise(vertex1WorldLoc));
			tris.Add(vertNumber);
			tris.Add(vertNumber + Grid.Instance.MoveDirFix(i) + 1);
			tris.Add(vertNumber + Grid.Instance.MoveDirFix(i + 1) + 1);
		}
		vertNumber += 7;
		uvs.Add(new Vector2(0.5f, 0.5f));
		uvs.Add(new Vector2(0.5f, 0));
		uvs.Add(new Vector2(1, 0));
		uvs.Add(new Vector2(1, 1));
		uvs.Add(new Vector2(0.5f, 1));
		uvs.Add(new Vector2(0, 1));
		uvs.Add(new Vector2(0, 0));
	}
	void AddSide(Tile tile)
	{
		for (int i = 0; i <= 5; i++)
		{
			Vector3 vertex1WorldLoc = tile.worldLoc + (Quaternion.Euler(0, (60 * i), 0) * Vector3.forward * Grid.Instance.hexRadius);
			Vector3 vertex2WorldLoc = tile.worldLoc + (Quaternion.Euler(0, (60 * Grid.Instance.MoveDirFix(i + 1)), 0) * Vector3.forward * Grid.Instance.hexRadius);
			Vector3 vertex3WorldLoc = vertex2WorldLoc;
			Vector3 vertex4WorldLoc = vertex1WorldLoc;

			float height1 = FindVertHeight(tile, i);
			float height2 = FindVertHeight(tile, i + 1);

			vertex1WorldLoc.y = height1;
			vertex2WorldLoc.y = height2;
			vertex3WorldLoc.y = height2 - data.heightMax;
			vertex4WorldLoc.y = height1 - data.heightMax;

			verts.Add(AddNoise(vertex2WorldLoc));
			verts.Add(AddNoise(vertex1WorldLoc));
			verts.Add(AddNoise(vertex4WorldLoc));
			verts.Add(AddNoise(vertex2WorldLoc));
			verts.Add(AddNoise(vertex4WorldLoc));
			verts.Add(AddNoise(vertex3WorldLoc));

			tris.Add(vertNumber);
			tris.Add(vertNumber + 1);
			tris.Add(vertNumber + 2);
			tris.Add(vertNumber + 3);
			tris.Add(vertNumber + 4);
			tris.Add(vertNumber + 5);
			vertNumber += 6;

			uvs.Add(new Vector2(1, 0));
			uvs.Add(new Vector2(1, 1));
			uvs.Add(new Vector2(0, 1));
			uvs.Add(new Vector2(1, 0));
			uvs.Add(new Vector2(0, 1));
			uvs.Add(new Vector2(0, 0));
		}
	}

	float FindVertHeight(Tile tile, int vertDir)
	{
		vertDir = Grid.Instance.MoveDirFix(vertDir);
		float height = tile.height;
		int connections = 1;
		Tile otherTile1 = null;
		Tile otherTile2 = null;
		if (data.tiles.ContainsKey(Grid.Instance.MoveTo(tile.gridLoc, vertDir)))
			otherTile1 = data.tiles[Grid.Instance.MoveTo(tile.gridLoc, vertDir)];
		if (data.tiles.ContainsKey(Grid.Instance.MoveTo(tile.gridLoc, vertDir + 1)))
			otherTile2 = data.tiles[Grid.Instance.MoveTo(tile.gridLoc, vertDir + 1)];
		if (otherTile1 != null)
		{
			if (tile.connections.Contains(otherTile1))
			{
				height += otherTile1.height;
				connections++;
				if(otherTile2 != null && !tile.connections.Contains(otherTile2) && otherTile1.connections.Contains(otherTile2))
				{
					height += otherTile2.height;
					connections++;
				}
			}
		}
		if (otherTile2 != null)
		{
			if (tile.connections.Contains(otherTile2))
			{
				height += otherTile2.height;
				connections++;
				if (otherTile1 != null && !tile.connections.Contains(otherTile1) && otherTile2.connections.Contains(otherTile1))
				{
					height += otherTile1.height;
					connections++;
				}
			}
		}
		height /= connections;
		return height;
	}

	Vector3 AddNoise(Vector3 worldLoc)
	{
		Vector3 noise = new Vector3(Mathf.PerlinNoise(0, worldLoc.z), Mathf.PerlinNoise(worldLoc.z, worldLoc.x), Mathf.PerlinNoise(worldLoc.x, 0));
		noise -= Vector3.one * 0.5f;
		noise *= 0.3f;
		if (addNoise)
			worldLoc += noise;
		return worldLoc;
	}
}
