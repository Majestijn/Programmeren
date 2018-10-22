using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(20)]
public class UnitManager : MonoBehaviour {

	public delegate void UnitDiesAction(Unit unit);

	public event UnitDiesAction OnUnitDeath;

	[SerializeField] private GameObject m_TestUnit;
	[SerializeField] private GameObject m_TestUnit1;
	
	public static UnitManager instance;

	List<Unit> m_UnitList;

	private void Awake()
	{
		instance = this;
	}

	void Start () {

		for (int i = 0; i < 5; i++)
		{
			Vector2Int poss = new Vector2Int(i, i);
			GameObject goo = Instantiate(m_TestUnit1, GridManager.instance.GetHexFromPosition(poss).transform.position, Quaternion.identity);
			goo.GetComponent<Unit>().m_CurrentHex = GridManager.instance.GetHexFromPosition(poss);
			goo.GetComponent<Unit>().m_CurrentHex.m_IsAvailable = false;
			GridManager.instance.GetHexFromPosition(poss).m_CurrentUnit = goo.GetComponent<Unit>();
			m_UnitList.Add(goo.GetComponent<Unit>());
		}

		Vector2Int pos = new Vector2Int(-3, 1);
		GameObject go = Instantiate(m_TestUnit, GridManager.instance.GetHexFromPosition(pos).transform.position, Quaternion.identity);
		go.GetComponent<Unit>().m_CurrentHex = GridManager.instance.GetHexFromPosition(pos);
		go.GetComponent<Unit>().m_CurrentHex.m_IsAvailable = false;
		GridManager.instance.GetHexFromPosition(pos).m_CurrentUnit = go.GetComponent<Unit>();
		m_UnitList.Add(go.GetComponent<Unit>());
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
		unit.m_CurrentHex.m_CurrentUnit = null;
		unit.m_CurrentHex.m_IsAvailable = true;
		GameObject.Destroy(unit.gameObject);
	}
}
