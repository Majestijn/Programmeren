using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

	#region variables
	public static GridManager instance;

	[SerializeField] private TextureCreator m_TextureCreator;

	[SerializeField] private List<BiomesWrapper> m_HexagonPrefab;
	[SerializeField] private Transform m_HexParent;

	[SerializeField] private int m_GridSize;

	public Dictionary<Vector2Int, Hex> m_HexDict;

	public static readonly Vector2Int[][] m_AxialDirections = new Vector2Int[][]
	{
		new Vector2Int[]
		{
			new Vector2Int(1,0),
			new Vector2Int(1,-1),
			new Vector2Int(0,-1),
			new Vector2Int(-1,0),
			new Vector2Int(0,1),
			new Vector2Int(1,1),
		},
		new Vector2Int[]
		{
			new Vector2Int(1,0),
			new Vector2Int(0,-1),
			new Vector2Int(-1,-1),
			new Vector2Int(-1,0),
			new Vector2Int(-1,1),
			new Vector2Int(0,1),
		}
	};
	#endregion

	private void Awake()
	{
		instance = this;
		m_HexDict = new Dictionary<Vector2Int, Hex>();
	}

	void Start () {
		Texture2D perlinTexture = m_TextureCreator.texture;

		#region Flat face hexagons
		//for (int x = 0; x < m_GridSize; x++)
		//{
		//	for (int y = 0; y < m_GridSize; y++)
		//	{
		//		GameObject go = Instantiate(m_HexagonPrefab);
		//		DrawHexagonLines drawcript = go.GetComponent<DrawHexagonLines>();
		//		m_HexTiles.Add(go, new Vector2Int(x, y));


		//		if (y % 2 == 0)
		//		{
		//			go.transform.position = new Vector3(x * drawcript.width, y * drawcript.height, 0);
		//		}
		//		else
		//		{
		//			go.transform.position = new Vector3(x * drawcript.width + (drawcript.width / 2), y * drawcript.height, 0);
		//		}
		//		go.transform.Rotate(-90f, 0f, 0f);
		//	}
		//}
		#endregion
		#region Pointy face hexagons
		//for (int x = -m_GridSize; x <= m_GridSize; x++)
		//{
		//	for (int y = -m_GridSize; y <= m_GridSize; y++)
		//	{
		//		if (Mathf.Abs(x + y) <= m_GridSize)
		//		{
		//			int randomTile = Random.Range(0, m_HexagonPrefab.Length);

		//			GameObject go = Instantiate(m_HexagonPrefab[randomTile], m_HexParent);
		//			Hex hex = go.GetComponent<Hex>();

		//			hex.m_GridPosition = new Vector2Int(x, y);

		//			//hex.SetTextCord(new Vector2Int(x, y));

		//			m_HexDict.Add(new Vector2Int(x, y), hex);

		//			Vector3 offset = new Vector3(x * hex.width, 0f, 0f);
		//			offset += new Vector3(y * hex.width / 2, 0f, y * -hex.height * 0.75f);

		//			go.transform.position = offset;
		//			go.transform.Rotate(0, -90f, 0);
		//		}
		//		else
		//		{
		//			Debug.Log("False");
		//		}
		//	}
		//}
		//Debug.Log(m_HexDict.Count);
		#endregion

		//First pass to randomly generate terrain
		for (int x = 0; x < m_GridSize; x++)
		{
			for (int y = 0; y < m_GridSize; y++)
			{
				TileType randomTile;			

				float grayscale = perlinTexture.GetPixel(x, y).grayscale;

				if (grayscale < 0.40f)
				{
					randomTile = TileType.Water;
				}
				else if (grayscale > 0.40f && grayscale < 0.70f)
				{
					randomTile = TileType.Grass;
				}
				else
				{
					randomTile = TileType.Mountain;
				}

				//Instantiate hex
				SpawnHexes(x, y, randomTile);
			}
		}

		//Second pass to make sure there are no singular water tiles
		int counter = 0;

		for (int x = 0; x < m_GridSize; x++)
		{
			for (int y = 0; y < m_GridSize; y++)
			{
				if (m_HexDict[new Vector2Int(x, y)].m_TileType == TileType.Water)
				{
					List<TileType> neighbourList = new List<TileType>();

					foreach (Hex neighbour in GetAllNeighbours(m_HexDict[new Vector2Int(x, y)]))
					{
						neighbourList.Add(neighbour.m_TileType);

						if (neighbour.m_TileType == TileType.Water) 
							counter ++;
					}
					
					//bool shouldDestroy = !neighbourList.Contains(TileType.Water);
					bool shouldDestroy = counter <= 2 || !neighbourList.Contains(TileType.Water);

					if (shouldDestroy)
					{
						ReplaceHex(new Vector2Int(x, y), TileType.Grass);
					}
				}
			}
		}
	}

	private void SpawnHexes(int x, int y, TileType tileNumber)
	{
		GameObject go = Instantiate(m_HexagonPrefab[(int)tileNumber].tiles[Random.Range(0, m_HexagonPrefab[(int)tileNumber].tiles.Count - 1)], m_HexParent);
		Hex hex = go.GetComponent<Hex>();

		switch (tileNumber)
		{
			case TileType.Grass:
				hex.m_IsAvailable = true;
				break;
			case TileType.Water:
				hex.m_IsAvailable = true;
				break;
			case TileType.Mountain:
				hex.m_IsAvailable = false;
				break;
		}

		//Add some hex values
		m_HexDict.Add(new Vector2Int(x, Mathf.Abs(y)), hex);

		hex.m_GridPosition = new Vector2Int(x, Mathf.Abs(y));

		hex.m_TileType = tileNumber;

		//Calculate hex position
		float offset = 0;

		if ((y & 1) == 0)
		{
			offset = hex.width / 2;
		}

		go.transform.position = new Vector3(x * hex.width + offset, (float)Random.Range(0f, 0.5f), y * hex.height * 0.75f);
		go.transform.Rotate(0, -90f, 0);
	}

	public Hex GetHexFromPosition(Vector2Int pos)
	{
		if (m_HexDict.ContainsKey(pos))
		{
			return m_HexDict[pos];
		}
		else
		{
			Debug.LogWarning("There is no hex found at the given position");
			return null;
		}
	}

	public List<Hex> GetNeighbours(Hex currentHex)
	{
		List<Hex> hexList = new List<Hex>();

		int parity = currentHex.m_GridPosition.y % 2;

		for (int i = 0; i < 6; i++)
		{
			Vector2Int tempPos = new Vector2Int(currentHex.m_GridPosition.x + m_AxialDirections[parity][i].x, currentHex.m_GridPosition.y + m_AxialDirections[parity][i].y);
			if (m_HexDict.ContainsKey(tempPos))
			{
				Hex hex = GetHexFromPosition(tempPos);

				if (hex.m_IsAvailable)
				{
					hexList.Add(hex);
				}
				else
				{
					if (hex.m_CurrentUnit != null)
					{
						hexList.Add(hex);
					}
				}
			}
		}
		return hexList;
	}

	public List<Hex> GetAllNeighbours(Hex currentHex)
	{
		List<Hex> hexList = new List<Hex>();

		int parity = currentHex.m_GridPosition.y % 2;

		for (int i = 0; i < 6; i++)
		{
			Vector2Int tempPos = new Vector2Int(currentHex.m_GridPosition.x + m_AxialDirections[parity][i].x, currentHex.m_GridPosition.y + m_AxialDirections[parity][i].y);
			if (m_HexDict.ContainsKey(tempPos))
			{
				Hex hex = GetHexFromPosition(tempPos);

				hexList.Add(hex);
			}
		}
		return hexList;
	}

	public Hex GetFirstAvailableSlotFromNeighbours(Vector2Int pos)
	{
		List<Hex> tempHexList = GetNeighbours(GetHexFromPosition(pos));

		foreach (Hex hex in tempHexList)
		{
			if (hex.m_IsAvailable && hex.m_TileType != TileType.Water)
			{
				return hex;
			}
		}

		return null;
	}

	public Hex FindRandomHex()
	{
		Hex hex = m_HexDict[new Vector2Int(Random.Range(0, m_GridSize), Random.Range(0, m_GridSize))];

		while (!hex.m_IsAvailable && hex.m_TileType == TileType.Water)
		{
			hex = m_HexDict[new Vector2Int(Random.Range(0, m_GridSize), Random.Range(0, m_GridSize))];
		}

		return hex;
	}

	public void ReplaceHex(Vector2Int hexPos, TileType tileType)
	{
		Transform tempTransform = m_HexDict[hexPos].transform;
		Destroy(m_HexDict[hexPos].gameObject);

		GameObject go = Instantiate(m_HexagonPrefab[(int)tileType].tiles[Random.Range(0, m_HexagonPrefab[(int)tileType].tiles.Count - 1)], m_HexParent);
		Hex hex = go.GetComponent<Hex>();

		hex.m_IsAvailable = true;
		m_HexDict[hexPos] = hex;
		hex.m_GridPosition = hexPos;

		hex.m_TileType = tileType;

		go.transform.position = tempTransform.position;
		go.transform.rotation = tempTransform.rotation;

	}

	public void BuildCity()
	{
		Vector2Int tempPos = SelectionManager.instance.m_CurrentlySelected.m_CurrentHex.m_GridPosition;
		ReplaceHex(tempPos, TileType.City);
		UnitManager.instance.DeleteUnit(SelectionManager.instance.m_CurrentlySelected);
		SelectionManager.instance.DeSelectUnit();

		CityNamer.instance.SetInputObject(true);
		CityNamer.instance.ChangeCity(m_HexDict[tempPos].GetComponent<City>());
	}

	public void BuildCity(Vector2Int pos)
	{
		ReplaceHex(pos, TileType.City);
		UnitManager.instance.DeleteUnit(SelectionManager.instance.m_CurrentlySelected);
		SelectionManager.instance.DeSelectUnit();

		CityNamer.instance.SetInputObject(true);
		CityNamer.instance.ChangeCity(m_HexDict[pos].GetComponent<City>());
	}

	public void ChangeHexMaterial(Hex hex, Material material)
	{
		hex.m_MeshRenderer.material = material;
	}

	public void ChangeHexMaterial(List<Hex> hexList, Material material)
	{
		foreach (Hex hex in hexList)
		{
			if (hex.m_CurrentUnit == null)
			{
				hex.m_MeshRenderer.material = material;
			}
		}
	}

	public void ResetAllHexes()
	{
		foreach( KeyValuePair<Vector2Int, Hex> keyValuePair in m_HexDict)
		{
			keyValuePair.Value.gCost = int.MaxValue;
			keyValuePair.Value.m_Parent = null;
		}
	}

	public int GetDistance(Vector2Int nodeA, Vector2Int nodeB)
	{
		Vector3 nodeAPos = OddToCube(nodeA);
		Vector3 nodeBPos = OddToCube(nodeB);
		int returnvalue = ((int)Mathf.Abs(nodeAPos.x - nodeBPos.x) + (int)Mathf.Abs(nodeAPos.y - nodeBPos.y) + (int)Mathf.Abs(nodeAPos.z - nodeBPos.z)) / 2;
		return returnvalue;
	}

	#region Coordinate conversion
	public Vector3 AxialToCube(Hex hex)
	{
		int x = hex.m_GridPosition.x;
		int z = hex.m_GridPosition.y;
		int y = -x - z;

		return new Vector3(x, y, z);
	}

	public Vector3 AxialToCube(Vector2Int pos)
	{
		int x = pos.x;
		int z = pos.y;
		int y = -x - z;

		return new Vector3(x, y, z);
	}

	public Vector3 OddToCube(Hex hex)
	{
		int x = hex.m_GridPosition.y - (hex.m_GridPosition.x + (hex.m_GridPosition.x & 1)) / 2;
		int z = hex.m_GridPosition.x;
		int y = -x - z;

		return new Vector3(x, y, z);
	}

	public Vector3 OddToCube(Vector2Int pos)
	{
		int x = pos.y - (pos.x + (pos.x & 1)) / 2;
		int z = pos.x;
		int y = -x - z;

		return new Vector3(x, y, z);
	}

	public int GetDistance(Hex nodeA, Hex nodeB)
	{
		Vector3 nodeAPos = OddToCube(nodeA);
		Vector3 nodeBPos = OddToCube(nodeB);
		int returnvalue = ((int)Mathf.Abs(nodeAPos.x - nodeBPos.x) + (int)Mathf.Abs(nodeAPos.y - nodeBPos.y) + (int)Mathf.Abs(nodeAPos.z - nodeBPos.z)) / 2;
		return returnvalue;
	}
	#endregion
}

public enum TileType
{
	Grass = 0,
	Water,
	Mountain,
	City
};

[System.Serializable]
public class BiomesWrapper
{
	public TileType tileType;
	public List<GameObject> tiles;
}
