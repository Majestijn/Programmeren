using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

	#region variables
	public static GridManager instance;

	[SerializeField] private GameObject m_HexagonPrefab;
	[SerializeField] private Transform m_HexParent;

	[SerializeField] private int m_GridSize;

	private List<LineRenderer> m_LineRendererList;

	public Dictionary<Vector2Int, Hex> m_HexDict;

	public static readonly Vector2Int[] m_AxialDirections = new Vector2Int[]
	{
		new Vector2Int(1, 0),
		new Vector2Int(1, -1),
		new Vector2Int(0, -1),
		new Vector2Int(-1, 0),
		new Vector2Int(-1, 1),
		new Vector2Int(0, 1),
	};
	#endregion

	private void Awake()
	{
		instance = this;
		m_HexDict = new Dictionary<Vector2Int, Hex>();
	}

	void Start () {
		m_LineRendererList = new List<LineRenderer>();

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
		for (int x = -m_GridSize; x < m_GridSize + 1; x++)
		{
			for (int y = -m_GridSize; y < m_GridSize + 1; y++)
			{
				if (Mathf.Abs(x + y) < m_GridSize + 1)
				{
					GameObject go = Instantiate(m_HexagonPrefab, m_HexParent);
					Hex hex = go.GetComponent<Hex>();

					hex.m_GridPosition = new Vector2Int(x, y);

					hex.SetTextCord(new Vector2Int(x, y));

					m_HexDict.Add(new Vector2Int(x, y), hex);

					Vector3 offset = new Vector3(x * hex.width, 0f, 0f);
					offset += new Vector3(y * hex.width / 2, 0f, y * -hex.height * 0.75f);

					go.transform.position = offset;
				}
			}
		}
		#endregion
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

		for (int i = 0; i < 6; i++)
		{
			Vector2Int tempPos = new Vector2Int(currentHex.m_GridPosition.x + GridManager.m_AxialDirections[i].x, currentHex.m_GridPosition.y + GridManager.m_AxialDirections[i].y);
			if (m_HexDict.ContainsKey(tempPos))
			{
				Hex hex = GetHexFromPosition(tempPos);
				hexList.Add(hex);

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

	public int GetDistance(Hex nodeA, Hex nodeB)
	{
		Vector3 nodeAPos = AxialToCube(nodeA);
		Vector3 nodeBPos = AxialToCube(nodeB);
		int returnvalue = ((int)Mathf.Abs(nodeAPos.x - nodeBPos.x) + (int)Mathf.Abs(nodeAPos.y - nodeBPos.y) + (int)Mathf.Abs(nodeAPos.z - nodeBPos.z)) / 2;
		return returnvalue;
	}
}
