﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(5)]
public class Bootstrap : MonoBehaviour {

	void Start () {

		//Spawn a settler and a warrior for the player
		Vector2Int randomPos = GridManager.instance.FindRandomHex().m_GridPosition;

		UnitManager.instance.InstantiateUnit(randomPos, AllyType.Settler);
		UnitManager.instance.InstantiateUnitAtPosition(GridManager.instance.GetFirstAvailableSlotFromNeighbours(randomPos).m_GridPosition, AllyType.WarriorCommon);

		//Spawn an enemy city
		Vector2Int enemyCityPos = GridManager.instance.FindRandomHex().m_GridPosition;

		while (GridManager.instance.GetDistance(randomPos, enemyCityPos) < 20)
		{
			enemyCityPos = GridManager.instance.FindRandomHex().m_GridPosition;
		}

		GridManager.instance.BuildCity(enemyCityPos, Allegiance.Enemy);
		UnitManager.instance.InstantiateUnitAtPosition(GridManager.instance.GetFirstAvailableSlotFromNeighbours(enemyCityPos).m_GridPosition, EnemyType.Spider);
		//UnitManager.instance.InstantiateUnitAtPosition(GridManager.instance.GetFirstAvailableSlotFromNeighbours(enemyCityPos).m_GridPosition, EnemyType.Spider);
		//UnitManager.instance.InstantiateUnitAtPosition(GridManager.instance.GetFirstAvailableSlotFromNeighbours(enemyCityPos).m_GridPosition, EnemyType.Spider);

	}
}
