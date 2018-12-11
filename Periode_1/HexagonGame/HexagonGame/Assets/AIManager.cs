using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(6)]
public class AIManager : MonoBehaviour
{

	void Start()
	{
		GameManager.instance.OnEndTurn += SendUnitsToTarget;
	}

	void Update()
	{

	}

	void SendUnitsToTarget()
	{
		//foreach (Unit enemy in UnitManager.instance.m_EnemyList)
		//{
		//	Debug.Log("Loop Executed");
		//	Hex target = null;
		//	int closestPosition = int.MaxValue;

		//	foreach (Unit ally in UnitManager.instance.m_AllyList)
		//	{
		//		if (GridManager.instance.GetDistance(enemy.m_CurrentHex, ally.m_CurrentHex) < closestPosition)
		//		{
		//			closestPosition = GridManager.instance.GetDistance(enemy.m_CurrentHex, ally.m_CurrentHex);
		//			target = ally.m_CurrentHex;
		//		}
		//	}

		//	List<Hex> hexList = Pathfinding.instance.FindPath(enemy.m_CurrentHex.m_GridPosition, target.m_GridPosition);
		//	GridManager.instance.ResetAllHexes();

		//	for (int i = 0; i < hexList.Count; i++)
		//	{
		//		if (GridManager.instance.GetDistance(enemy.m_CurrentHex, hexList[i]) == enemy.m_MoveAmount + 1)
		//		{
		//			target = hexList[i];
		//		}
		//	}

		//	enemy.MoveToHex(target);
		//}

		StartCoroutine(RSendUnitsToTarget(UnitManager.instance.m_EnemyList.Count));
	}

	private IEnumerator RSendUnitsToTarget(int counter)
	{
		for (int i = 0; i < counter; i++)
		{
			Debug.Log("Loop " + i + " executed");

			GridManager.instance.m_CanMove = false;

			Unit enemy = UnitManager.instance.m_EnemyList[i];
			Hex target = null;
			int closestPosition = int.MaxValue;

			foreach (Unit ally in UnitManager.instance.m_AllyList)
			{
				if (GridManager.instance.GetDistance(enemy.m_CurrentHex, ally.m_CurrentHex) < closestPosition)
				{
					closestPosition = GridManager.instance.GetDistance(enemy.m_CurrentHex, ally.m_CurrentHex);
					target = ally.m_CurrentHex;
				}
			}

			List<Hex> hexList = Pathfinding.instance.FindPath(enemy.m_CurrentHex.m_GridPosition, target.m_GridPosition);

			for (int x = 0; x < hexList.Count; x++)
			{
				if (GridManager.instance.GetDistance(enemy.m_CurrentHex, hexList[x]) == enemy.m_MoveAmount)
				{
					target = hexList[x];
				}
			}

			enemy.MoveToHex(target);

			while (!GridManager.instance.m_CanMove && i <= counter)
			{
				yield return null;
			}
		}
		yield return null;
	}
}
