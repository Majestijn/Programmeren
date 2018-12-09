using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(6)]
public class AIManager : MonoBehaviour {

	void Start () {
		GameManager.instance.OnEndTurn += SendUnitsToTarget;
	}
	
	void Update () {
		
	}

	void SendUnitsToTarget()
	{
		foreach (Unit enemy in UnitManager.instance.m_EnemyList)
		{
			Unit target = null;
			int closestPosition = int.MaxValue;

			foreach (Unit ally in UnitManager.instance.m_AllyList)
			{
				if (GridManager.instance.GetDistance(enemy.m_CurrentHex, ally.m_CurrentHex) < closestPosition)
				{
					closestPosition = GridManager.instance.GetDistance(enemy.m_CurrentHex, ally.m_CurrentHex);
					target = ally;
				}
			}

			List<Hex> hexList = Pathfinding.instance.FindPath(enemy.m_CurrentHex.m_GridPosition, target.m_CurrentHex.m_GridPosition);

			for (int i = 0; i < hexList.Count; i++)
			{
				if (GridManager.instance.GetDistance(enemy.m_CurrentHex, hexList[i]) == enemy.m_MoveAmount)
				{
					target = hexList[i].m_CurrentUnit;
				}
			}

			enemy.MoveToHex(target.m_CurrentHex);
		}
	}
}
