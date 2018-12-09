using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DefaultExecutionOrder(0)]
public class SelectionManager : MonoBehaviour {

	public delegate void UnitSelectionAction(Unit unit);
	public delegate void UnitDeselectionAction();

	public delegate void CitySelectionAction(City city);
	public delegate void CityDeselectionAction();

	public event UnitSelectionAction OnUnitSelected;
	public event UnitDeselectionAction OnUnitDeselected;

	public event CitySelectionAction OnCitySelected;
	public event CityDeselectionAction OnCityDeselected;

	#region variables
	public static SelectionManager instance;

	public GameObject m_SelectionOutline;

	public Unit m_CurrentlySelected;
	public City m_CurrentlySelectedCity;

	private List<Hex> m_AllMovementPossibilities;

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
		if (Input.GetMouseButtonDown(0) && !HelperClass.IsPointerOverUIElement())
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
			{
				Hex hex = hit.transform.gameObject.GetComponent<Hex>();
				Unit unit = hex.m_CurrentUnit;
				City city = hex.GetComponent<City>();

				if (hex != null)
				{
					if (unit != null && m_CanSelect && !unit.CheckIfDead() && unit.unitData.alligiance == Allegiance.Ally)
					{
						SelectUnit(hex, unit);

						if (city != null)
							SelectCity(city);
					}
					else if (city != null && m_CanSelect && city.GetCityAllegience() == Allegiance.Ally)
					{
						SelectCity(city);
						DeSelectUnit();
					}
					else
					{
						DeselectCity();
						DeSelectUnit();
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

					if (hex != null && m_CurrentlySelected != null && m_CurrentlySelected.m_MoveAmount > 0 && m_CurrentlySelected.m_CurrentHex != hex && m_AllMovementPossibilities.Contains(hex))
					{
						if (hex.m_CurrentUnit != null && hex.m_CurrentUnit.unitData.alligiance != m_CurrentlySelected.unitData.alligiance)
						{
							m_CanSelect = false;
							m_CurrentlySelected.MoveToEnemy(hex);
						}
						else
						{
							if (hex.m_IsAvailable && m_CanSelect)
							{
								m_CanSelect = false;
								m_CurrentlySelected.MoveToHex(hex);
							}
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

	private void DisplayMovementPossibilities(Hex startPos, Hex targetPos)
	{
		if (m_AllMovementPossibilities != null)
		{
			GridManager.instance.ChangeHexMaterial(m_AllMovementPossibilities, MaterialManager.instance.GetMaterial(MaterialName.BaseMaterial));
		}

		GridManager.instance.ResetAllHexes();
		m_AllMovementPossibilities = Pathfinding.instance.FindPath(startPos.m_GridPosition, targetPos.m_GridPosition);
		GridManager.instance.ChangeHexMaterial(m_AllMovementPossibilities, MaterialManager.instance.GetMaterial(MaterialName.SelectedMaterial));
	}

	public void SelectUnit()
	{
		if (m_CurrentlySelected != null)
		{
			m_CanSelect = true;
			DisplayMovementPossibilities(m_CurrentlySelected.m_CurrentHex, m_CurrentlySelected.m_MoveAmount);
			OnUnitSelected?.Invoke(m_CurrentlySelected);
			m_CurrentlySelected.m_AllMovementPossibilities = m_AllMovementPossibilities;
		}
	}

	public void SelectUnit(Hex target)
	{
		if (m_CurrentlySelected != null)
		{
			m_CanSelect = true;
			DisplayMovementPossibilities(m_CurrentlySelected.m_CurrentHex, target);
			OnUnitSelected?.Invoke(m_CurrentlySelected);
			m_CurrentlySelected.m_AllMovementPossibilities = m_AllMovementPossibilities;
		}
	}

	public void SelectCity(City city)
	{
		OnCitySelected?.Invoke(city);
		m_CurrentlySelectedCity = city;
	}

	public void DeselectCity()
	{
		OnCityDeselected?.Invoke();
		m_CurrentlySelectedCity = null;
	}

	public void DeSelectUnit()
	{
		OnUnitDeselected?.Invoke();

		if (m_AllMovementPossibilities != null)
		{
			GridManager.instance.ChangeHexMaterial(m_AllMovementPossibilities, MaterialManager.instance.GetMaterial(MaterialName.BaseMaterial));
		}

		m_CurrentlySelected = null;
	}

	public void SelectUnit(Hex hex, Unit unit)
	{
		m_CanSelect = true;
		DisplayMovementPossibilities(hex, hex.m_CurrentUnit.m_MoveAmount);
		m_CurrentlySelected = unit;
		OnUnitSelected?.Invoke(unit);
		unit.m_AllMovementPossibilities = m_AllMovementPossibilities;
	}
}
