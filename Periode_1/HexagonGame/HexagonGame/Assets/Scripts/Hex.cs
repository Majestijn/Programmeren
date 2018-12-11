using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hex : MonoBehaviour
{

	#region variables

	private GameObject m_TextObject;
	private TextMeshPro m_TextMesh;

	private float m_Size;
	private int m_GCost = int.MaxValue;
	private int m_HCost;

	[HideInInspector]
	public float width, height;

	[HideInInspector]
	public Vector2Int m_GridPosition;

	[HideInInspector]
	public MeshRenderer m_MeshRenderer;

	public bool m_IsAvailable = true;
	[HideInInspector]
	public Hex m_Parent;
	[HideInInspector]
	public Unit m_CurrentUnit;
	[HideInInspector]
	public TileType m_TileType;

	#region Getters
	public int gCost
	{
		get { return m_GCost; }
		set { m_GCost = value; }
	}
	public int hCost
	{
		get { return m_HCost; }
		set { m_HCost = value; }
	}

	public int fCost
	{
		get { return m_GCost + m_HCost; }
	}
	#endregion
	#endregion

	private void Awake()
	{
		//m_TextObject = transform.GetChild(0).gameObject;
		//m_TextMesh = m_TextObject.GetComponent<TextMeshPro>();

		m_MeshRenderer = GetComponent<MeshRenderer>();

		MeshFilter mesh = GetComponent<MeshFilter>();

		//m_Size = 5f;

		//width = Mathf.Sqrt(3f) * m_Size;
		//height = (2 * m_Size);

		//width = Mathf.Sqrt(3f) * mesh.mesh.bounds.size.x;
		//height = (2 * mesh.mesh.bounds.size.z);

		width = mesh.mesh.bounds.size.z;
		height = mesh.mesh.bounds.size.x;
	}

	void Start()
	{
		//Generate3DMesh();
		//Generate2DMesh();
	}

	public Vector2 CalculateVerts(Vector2 center, float size, int i)
	{
		float angle_deg = 60 * i - 30;
		float angle_rad = Mathf.PI / 180 * angle_deg;
		return new Vector2(center.x + size * Mathf.Cos(angle_rad), center.y + size * Mathf.Sin(angle_rad));
	}

	public void SetTextCord(Vector2Int cord)
	{
		m_TextMesh.text = cord.x + "  " + cord.y;
	}

	public void ResetValues()
	{
		m_CurrentUnit = null;
		m_IsAvailable = true;
	}

	private void Generate3DMesh()
	{
		float randomHeight = Random.Range(0f, 0f);

		Mesh mesh = new Mesh();
		Vector3[] vertices = new Vector3[14];
		vertices[0] = new Vector3(0, randomHeight, 0);
		vertices[7] = Vector3.zero;

		for (int i = 0; i < 6; i++)
		{
			float angle_deg = 60 * i - 30;
			float angle_rad = Mathf.Deg2Rad * angle_deg;

			vertices[i + 1] = new Vector3(m_Size * Mathf.Cos(angle_rad), randomHeight, m_Size * Mathf.Sin(angle_rad));
			vertices[i + 8] = new Vector3(m_Size * Mathf.Cos(angle_rad), 0f, -m_Size * Mathf.Sin(angle_rad));
		}

		//m_TextObject.transform.position = new Vector3(m_TextObject.transform.position.x, vertices[0].y + 2f, m_TextObject.transform.position.z);

		mesh.vertices = vertices;

		mesh.triangles = new int[]
		{
			2, 1, 0,
			3, 2, 0,
			4, 3, 0,
			5, 4, 0,
			6, 5, 0,
			1, 6, 0,

			9, 8, 7,
			10, 9, 7,
			11, 10, 7,
			12, 11, 7,
			13, 12, 7,
			8, 13, 7,

			1, 2, 8,
			1, 8, 9,

			2, 3, 8,
			13, 8, 3,

			3, 4, 13,
			12, 13, 4,

			4, 5, 12,
			11, 12, 5,

			5, 6, 11,
			10, 11, 6,

			6, 1, 10,
			9, 10, 1
		};

		Vector2[] uv = new Vector2[14];

		for (int i = 0; i < 14; i++)
		{
			uv[i] = new Vector2(
				(vertices[i].x + -width * .5f) * .5f / m_Size,
				(vertices[i].z + -height * .5f) * .5f / m_Size);
		}

		mesh.uv = uv;

		GetComponent<MeshFilter>().mesh = mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;

		m_MeshRenderer = GetComponent<MeshRenderer>();
	}

	private void Generate2DMesh()
	{
		Mesh mesh = new Mesh();
		Vector3[] vertices = new Vector3[7];
		vertices[0] = Vector3.zero;

		for (int i = 0; i < 6; i++)
		{
			float angle_deg = 60 * i - 30;
			float angle_rad = Mathf.Deg2Rad * angle_deg;

			vertices[i + 1] = new Vector3(m_Size * Mathf.Cos(angle_rad), 0f, m_Size * Mathf.Sin(angle_rad));
		}

		mesh.vertices = vertices;

		mesh.triangles = new int[]
		{
			2, 1, 0,
			3, 2, 0,
			4, 3, 0,
			5, 4, 0,
			6, 5, 0,
			1, 6, 0,
		};

		Vector2[] uv = new Vector2[7];

		for (int i = 0; i < 7; i++)
		{
			uv[i] = new Vector2(
				(vertices[i].x + -width * .5f) * .5f / m_Size,
				(vertices[i].z + -height * .5f) * .5f / m_Size);
		}

		mesh.uv = uv;

		GetComponent<MeshFilter>().mesh = mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;
	}

	private void OnMouseEnter()
	{
		if (!HelperClass.IsPointerOverUIElement())
		{
			SelectionManager.instance.m_SelectionOutline.transform.position = transform.position;
		}
	}
}
