using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(3)]
public class UnitManager : MonoBehaviour {

	public delegate void UnitDiesAction(Unit unit);

	public event UnitDiesAction OnUnitDeath;
	
	public static UnitManager instance;

	public List<Unit> m_AllyList;
	public List<Unit> m_EnemyList;

	private void Awake()
	{
		instance = this;
	}

	void Start () {

		GameManager.instance.OnEndTurn += ResetAllUnits;

		m_AllyList = new List<Unit>();
		m_EnemyList = new List<Unit>();
	}

	private void OnDestroy()
	{
		GameManager.instance.OnEndTurn -= ResetAllUnits;
	}

	#region Instantiate Unit Functions
	public void InstantiateUnit(Vector2Int fromWhere, AllyType type)
	{
		Hex hex = GridManager.instance.GetFirstAvailableSlotFromNeighbours(fromWhere);

		if (hex != null)
		{
			Vector3 tempPos = hex.transform.position;

			GameObject go = Instantiate(UnitLibrary.instance.GetAllyData(type), tempPos, Quaternion.identity);
			Unit unit = go.GetComponent<Unit>();

			unit.m_CurrentHex = hex;
			unit.m_CurrentHex.m_IsAvailable = false;

			hex.m_CurrentUnit = unit;

			m_AllyList.Add(unit);
		}
		else
		{
			UIManager.instance.ToggleMoreSpaceObject();
		}

	}
	public void InstantiateUnitAtPosition(Vector2Int where, AllyType type)
	{
		Hex hex = GridManager.instance.m_HexDict[where];
		Vector3 tempPos = hex.transform.position;

		GameObject go = Instantiate(UnitLibrary.instance.GetAllyData(type), tempPos, Quaternion.identity);
		Unit unit = go.GetComponent<Unit>();

		unit.m_CurrentHex = hex;
		unit.m_CurrentHex.m_IsAvailable = false;

		hex.m_CurrentUnit = unit;

		m_AllyList.Add(unit);
	}
	public void InstantiateUnit(Vector2Int fromWhere, EnemyType type)
	{
		Hex hex = GridManager.instance.GetFirstAvailableSlotFromNeighbours(fromWhere);
		Vector3 tempPos = hex.transform.position;

		GameObject go = Instantiate(UnitLibrary.instance.GetEnemyData(type), tempPos, Quaternion.identity);
		Unit unit = go.GetComponent<Unit>();

		unit.m_CurrentHex = hex;
		unit.m_CurrentHex.m_IsAvailable = false;

		hex.m_CurrentUnit = unit;

		if (unit.unitData.alligiance == Allegiance.Ally)
			m_AllyList.Add(unit);
		else
			m_EnemyList.Add(unit);
	}
	public void InstantiateUnitAtPosition(Vector2Int where, EnemyType type)
	{
		Hex hex = GridManager.instance.m_HexDict[where];
		Vector3 tempPos = hex.transform.position;

		GameObject go = Instantiate(UnitLibrary.instance.GetEnemyData(type), tempPos, Quaternion.identity);
		Unit unit = go.GetComponent<Unit>();

		unit.m_CurrentHex = hex;
		unit.m_CurrentHex.m_IsAvailable = false;

		hex.m_CurrentUnit = unit;

		m_EnemyList.Add(unit);
	}
	#endregion

	public void BuyWarrior()
	{
		InstantiateUnit(SelectionManager.instance.m_CurrentlySelectedCity.m_Hex.m_GridPosition, AllyType.WarriorCommon);
	}

	public void BuyArcher()
	{
		InstantiateUnit(SelectionManager.instance.m_CurrentlySelectedCity.m_Hex.m_GridPosition, AllyType.ArcherCommon);
	}

	public void BuyMage()
	{
		InstantiateUnit(SelectionManager.instance.m_CurrentlySelectedCity.m_Hex.m_GridPosition, AllyType.MageCommon);
	}

	public void FireUnitDiesAction(Unit unit)
	{
		DeleteUnit(unit);
		OnUnitDeath?.Invoke(unit);
	}

	public void DeleteUnit(Unit unit) 
	{
		unit.m_Animator.SetTrigger("Die");
		StartCoroutine(RDeleteUnit(unit));
	}

	IEnumerator RDeleteUnit(Unit unit)
	{
		yield return new WaitForSeconds(4);
		unit.m_CurrentHex.ResetValues();
		GameObject.Destroy(unit.gameObject);
	}

	public void ResetAllUnits()
	{
		foreach (Unit unit in m_AllyList)
		{
			unit.m_MoveAmount = unit.unitData.moveAmount;
		}

		foreach (Unit unit in m_EnemyList)
		{
			unit.m_MoveAmount = unit.unitData.moveAmount;
		}
	}
}
