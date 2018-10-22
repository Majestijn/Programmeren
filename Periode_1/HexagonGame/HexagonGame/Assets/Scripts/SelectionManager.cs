﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(10)]
public class SelectionManager : MonoBehaviour {

	public delegate void UnitSelectionAction(Unit unit);
	public delegate void UnitDeselectionAction();

	public event UnitSelectionAction OnUnitSelected;
	public event UnitDeselectionAction OnUnitDeselected;

	#region variables
	public static SelectionManager instance;

	Unit m_CurrentlySelected;

	List<Hex> m_AllMovementPossibilities;

	public bool m_CanSelect = true;
	#endregion

	private void Awake()
	{
		instance = this;
	}

	private void Update()
	{
		SelectionInput();
		ClickInput();
	}

	private void SelectionInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
			{
				Hex hex = hit.transform.gameObject.GetComponent<Hex>();
				Unit unit = hex.m_CurrentUnit;

				if (hex != null)
				{
					if (unit != null && m_CanSelect && !unit.CheckIfDead())
					{
						DisplayMovementPossibilities(hex, hex.m_CurrentUnit.m_MoveAmount);
						m_CurrentlySelected = unit;
						OnUnitSelected?.Invoke(unit);
						unit.m_AllMovementPossibilities = m_AllMovementPossibilities;
					}
					else
					{
						OnUnitDeselected?.Invoke();
						RemoveMovementPossibilities();
						m_CurrentlySelected = null;
					}
				}
				else
				{
					Debug.Log("Hex is null");
				}
			}
		}
	}

	private void ClickInput()
	{
		if (Input.GetMouseButtonDown(1))
		{
			if (m_CurrentlySelected != null)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;

				if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
				{
					Hex hex = hit.transform.GetComponent<Hex>();

					if (hex != null && m_CurrentlySelected != null && m_CurrentlySelected.m_MoveAmount > 0 && m_CurrentlySelected.m_CurrentHex != hex)
					{
						if (hex.m_CurrentUnit != null)
						{
							m_CanSelect = false;
							m_CurrentlySelected.MoveToEnemy(hex);
						}
						else
						{
							m_CanSelect = false;
							m_CurrentlySelected.MoveToHex(hex);
						}
					}
				}
				Debug.Log("Right mouse button was clicked!");
			}
		}
	}

	private void DisplayMovementPossibilities(Hex startPos, int moveAmount)
	{
		if (m_AllMovementPossibilities != null)
		{
			GridManager.instance.ChangeHexMaterial(m_AllMovementPossibilities, MaterialManager.instance.GetMaterial(MaterialName.BaseMaterial));
		}

		GridManager.instance.ResetAllHexes();
		m_AllMovementPossibilities = Pathfinding.instance.GetUnitRange(startPos, moveAmount);
		GridManager.instance.ChangeHexMaterial(m_AllMovementPossibilities, MaterialManager.instance.GetMaterial(MaterialName.SelectedMaterial));
	}

	private void RemoveMovementPossibilities()
	{
		if (m_AllMovementPossibilities != null)
		{
			GridManager.instance.ChangeHexMaterial(m_AllMovementPossibilities, MaterialManager.instance.GetMaterial(MaterialName.BaseMaterial));
		}
	}
}