using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

	#region variables
	public static GridManager instance;

	[SerializeField] private List<BiomesWrapper> m_HexagonPrefab;
	[SerializeField] private Transform m_HexParent;

	[SerializeField] private int m_GridSize;

	private List<LineRenderer> m_LineRendererList;

	public Dictionary<Vector2Int, Hex> m_HexDict;

	public Material m_Material;

	private List<Vector2Int> m_GridList;

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
		m_LineRendererList = new List<LineRenderer>();

		float absoluteGridSize = (m_GridSize * 2) * (m_GridSize * 2);
		int referencePoints = (int)absoluteGridSize / (int)Mathf.Pow(m_GridSize, 2f) * 2;

		Texture2D perlinTexture = GeneratePerlinNoiseTexture(m_GridSize, m_GridSize, 10);

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

		for (int x = 0; x < m_GridSize; x++)
		{
			for (int y = 0; y > -m_GridSize; y--)
			{
				int randomTile = 0;			

				float grayscale = perlinTexture.GetPixel(x, y).grayscale * 255;

				if (grayscale < 150)
				{
					randomTile = 2;
				}
				else
				{
					randomTile = 1;
				}

				//Instantiate hex
				SpawnHexes(x, y, randomTile);
			}
		}
	}

	private void SpawnHexes(int x, int y, int tileNumber)
	{
		GameObject go = Instantiate(m_HexagonPrefab[tileNumber].tiles[Random.Range(0, m_HexagonPrefab[tileNumber].tiles.Count - 1)], m_HexParent);
		Hex hex = go.GetComponent<Hex>();

		switch (tileNumber)
		{
			case 1:
				hex.m_IsAvailable = false;
				break;
			case 2:
				hex.m_IsAvailable = true;
				break;
			case 3:
				hex.m_IsAvailable = false;
				break;
		}

		//Add some hex values
		m_HexDict.Add(new Vector2Int(x, Mathf.Abs(y)), hex);

		hex.m_GridPosition = new Vector2Int(x, Mathf.Abs(y));

		//Calculate hex position
		float offset = 0;

		if (y % 2 == 0)
		{
			offset = hex.width / 2;
		}

		//hex.SetTextCord(new Vector2Int(x, Mathf.Abs(y)));

		go.transform.position = new Vector3(x * hex.width + offset, (float)Random.Range(0f, 1f), y * hex.height * 0.75f);
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
		Debug.Log(parity);

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
			}
		}
		return hexList;
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

	public Vector3 AxialToCube(Hex hex)
	{
		int x = hex.m_GridPosition.x;
		int z = hex.m_GridPosition.y;
		int y = -x-z;

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
		int y = -x-z;

		return new Vector3(x, y, z);
	}

	public Vector3 OddToCube(Vector2Int pos)
	{
		int x = pos.y - (pos.x + (pos.x&1)) / 2;
		int z = pos.x;
		int y = -x-z;

		return new Vector3(x, y, z);
	}

	public int GetDistance(Hex nodeA, Hex nodeB)
	{
		Vector3 nodeAPos = OddToCube(nodeA);
		Vector3 nodeBPos = OddToCube(nodeB);
		int returnvalue = ((int)Mathf.Abs(nodeAPos.x - nodeBPos.x) + (int)Mathf.Abs(nodeAPos.y - nodeBPos.y) + (int)Mathf.Abs(nodeAPos.z - nodeBPos.z)) / 2;
		return returnvalue;
	}

	public int GetDistance(Vector2Int nodeA, Vector2Int nodeB)
	{
		Vector3 nodeAPos = OddToCube(nodeA);
		Vector3 nodeBPos = OddToCube(nodeB);
		int returnvalue = ((int)Mathf.Abs(nodeAPos.x - nodeBPos.x) + (int)Mathf.Abs(nodeAPos.y - nodeBPos.y) + (int)Mathf.Abs(nodeAPos.z - nodeBPos.z)) / 2;
		return returnvalue;
	}

	private Texture2D GeneratePerlinNoiseTexture(int width, int height, int scale)
	{
		Texture2D texture = new Texture2D(width, height);

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Color color = CalculatePerlinNoise(x, y, width, height, scale);
				texture.SetPixel(x, y, color);
			}
		}

		texture.Apply();

		return texture;
	}

	private Texture2D GenerateGradientTexture(int width, int height, int scale)
	{
		Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, true);
		Vector2 center = new Vector2(width * 0.5f, height * 0.5f);

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				float distFromCenter = Vector2.Distance(center, new Vector2(x, y));
				float pixel = (0.3f / (distFromCenter / width));
				texture.SetPixel(x, y, new Color(pixel, pixel, pixel));
			}
		}

		texture.Apply();
		return texture;
	}

	private Color CalculatePerlinNoise(int x, int y, int width, int height, int scale)
	{
		float xcoord = (float)x / width * scale;
		float ycoord = (float)y / height * scale;

		float colorSample = Mathf.PerlinNoise(xcoord + Random.Range(0, 1000), ycoord + Random.Range(0, 1000));
		return new Color(colorSample, colorSample, colorSample);
	}
}

[System.Serializable]
public class BiomesWrapper
{
	public List<GameObject> tiles;
}
