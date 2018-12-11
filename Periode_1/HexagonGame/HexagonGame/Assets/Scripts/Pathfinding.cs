using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(20)]
public class Pathfinding : MonoBehaviour
{

	public static Pathfinding instance;

	private void Start()
	{
		instance = this;
	}

	public List<Hex> FindPath(Vector2Int startPos, Vector2Int targetPos)
	{
		Hex startNode = GridManager.instance.GetHexFromPosition(startPos);
		Hex targetNode = GridManager.instance.GetHexFromPosition(targetPos);

		List<Hex> openList = new List<Hex>();

		startNode.gCost = 0;
		openList.Add(startNode);

		do
		{
			Hex currentNode = GetHexWithLowestGCost(openList);

			openList.Remove(currentNode);


			List<Hex> currentNodeNeighbours = GridManager.instance.GetNeighbours(currentNode);

			foreach (Hex neighbour in currentNodeNeighbours)
			{
				if (currentNode.gCost + 1 < neighbour.gCost)
				{
					neighbour.gCost = currentNode.gCost + 1;
					neighbour.hCost = GridManager.instance.GetDistance(currentNode, targetNode);

					neighbour.m_Parent = currentNode;

					if (neighbour == targetNode)
					{
						return RetracePath(startNode, targetNode);
					}

					openList.Add(neighbour);
				}
			}
		}
		while (openList.Count > 0);

		Debug.Log("No path was found");
		return null;
	}

	public List<Hex> GetUnitRange(Hex startNode, int moveAmount)
	{
		Queue<Hex> openList = new Queue<Hex>();

		startNode.gCost = 0;
		startNode.m_Parent = null;
		openList.Enqueue(startNode);

		while (openList.Count > 0)
		{
			List<Hex> neighbours = GridManager.instance.GetNeighbours(openList.Peek());

			foreach (Hex neighbour in neighbours)
			{
				if (openList.Peek().gCost + 1 < neighbour.gCost && openList.Peek().gCost <= moveAmount)
				{
					neighbour.gCost = openList.Peek().gCost + 1;
					neighbour.m_Parent = openList.Peek();

					openList.Enqueue(neighbour);
				}
			}

			openList.Dequeue();
		}

		List<Hex> moveOptions = new List<Hex>();

		foreach (KeyValuePair<Vector2Int, Hex> keyValuePair in GridManager.instance.m_HexDict)
		{
			if (keyValuePair.Value.gCost <= moveAmount)
			{
				if (keyValuePair.Value != startNode)
				{
					moveOptions.Add(keyValuePair.Value);
				}
			}
		}
		return moveOptions;
	}

	public List<Hex> RetracePath(Hex startNode, Hex targetNode)
	{
		List<Hex> path = new List<Hex>();
		Hex currentNode = targetNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.m_Parent;
		}

		path.Reverse();
		return path;
	}

	Hex GetHexWithLowestGCost(List<Hex> hexList)
	{
		Hex hexWithLowestCost = null;
		int lowestCost = int.MaxValue;

		foreach (Hex hex in hexList)
		{
			if (hex.gCost < lowestCost)
			{
				lowestCost = hex.gCost;
				hexWithLowestCost = hex;
			}
		}

		return hexWithLowestCost;
	}
}
