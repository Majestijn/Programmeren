using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IMovable {

	[SerializeField] private UnitData m_Data;

	private Health m_Health;
	private Health m_TargetHealth;

	private int m_AttackDamage;

	public int m_MoveAmount;

	private int m_CurrentHexCount = 0;

	public List<Hex> m_AllMovementPossibilities;
	private List<Hex> m_PathList;

	public Hex m_CurrentHex;

	public Animator m_Animator;

	#region Getters
	public Health healthScript
	{
		get { return m_Health; }
	}

	public UnitData unitData
	{
		get { return m_Data; }
	}
	#endregion

	void Start () {
		m_Health = GetComponent<Health>();
		m_Health.SetCurrentHealth(m_Data.health);
		m_Health.OnUnitDeath += Die;

		m_AttackDamage = m_Data.attackDamage;
		m_MoveAmount = m_Data.moveAmount;

		m_Animator = GetComponent<Animator>();
	}

	public void MoveToHex(Hex targetHex)
	{
		StartCoroutine(RMoveToHex(targetHex));
	}

	public void MoveToEnemy(Hex targetHex)
	{
		StartCoroutine(RAttackEnemy(targetHex));
	}

	private IEnumerator RMoveToHex(Hex targetHex)
	{
		m_Animator.SetBool("IsWalking", true);

		GridManager.instance.ChangeHexMaterial(m_AllMovementPossibilities, MaterialManager.instance.GetMaterial(MaterialName.BaseMaterial));

		m_PathList = Pathfinding.instance.RetracePath(m_CurrentHex, targetHex);
		GridManager.instance.ChangeHexMaterial(m_PathList, MaterialManager.instance.GetMaterial(MaterialName.SelectedLineMaterial));

		m_CurrentHexCount = 0;

		while (Vector3.Distance(transform.position, m_PathList[m_PathList.Count - 1].transform.position) > 0.2f)
		{
			if (Vector3.Distance(transform.position, m_PathList[m_CurrentHexCount].transform.position) > 0.2f)
			{
				Vector3 direction = m_PathList[m_CurrentHexCount].transform.position - transform.position;

				Vector3 targetRot = transform.position - m_PathList[m_CurrentHexCount].transform.position;
				Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, 10 * Time.deltaTime, 0.0f);

				transform.rotation = Quaternion.LookRotation(newDir);
				transform.Translate(direction.normalized * Time.deltaTime * 15, Space.World);
				yield return new WaitForEndOfFrame();
			}
			else
			{
				if (m_CurrentHexCount + 1 < m_PathList.Count)
				{
					GridManager.instance.ChangeHexMaterial(m_PathList[m_CurrentHexCount], MaterialManager.instance.GetMaterial(MaterialName.BaseMaterial));
					m_CurrentHexCount++;
					yield return new WaitForEndOfFrame();
				}
				yield return new WaitForEndOfFrame();
			}
		}

		m_Animator.SetBool("IsWalking", false);

		m_MoveAmount -= m_PathList.Count;

		m_CurrentHex.m_IsAvailable = true;
		m_CurrentHex.m_CurrentUnit = null;
		m_CurrentHex = m_PathList[m_PathList.Count - 1];
		m_CurrentHex.m_IsAvailable = false;
		m_CurrentHex.m_CurrentUnit = this;

		m_PathList.Clear();

		GridManager.instance.ChangeHexMaterial(m_CurrentHex, MaterialManager.instance.GetMaterial(MaterialName.BaseMaterial));

		SelectionManager.instance.SelectUnit(m_CurrentHex, this);

		yield return null;
	}

	private IEnumerator RAttackEnemy(Hex targetHex)
	{
		#region Check if one of the current neighbours isn't already the target
		List<Hex> neighbours = GridManager.instance.GetNeighbours(m_CurrentHex);
		
		foreach(Hex hex in neighbours)
		{
			if (hex == targetHex)
			{
				Debug.Log("Already next to target");

				m_TargetHealth = hex.m_CurrentUnit.healthScript;
				transform.LookAt(hex.transform);
				Attack();

				yield break;
			}
		}
		#endregion

		m_Animator.SetBool("IsWalking", true);

		GridManager.instance.ChangeHexMaterial(m_AllMovementPossibilities, MaterialManager.instance.GetMaterial(MaterialName.BaseMaterial));

		m_PathList = Pathfinding.instance.RetracePath(m_CurrentHex, targetHex);

		Transform transformToLookAt = m_PathList[m_PathList.Count - 1].transform;

		m_PathList.RemoveAt(m_PathList.Count - 1);


		GridManager.instance.ChangeHexMaterial(m_PathList, MaterialManager.instance.GetMaterial(MaterialName.SelectedLineMaterial));

		m_CurrentHexCount = 0;

		while (Vector3.Distance(transform.position, m_PathList[m_PathList.Count - 1].transform.position) > 0.2f)
		{
			if (Vector3.Distance(transform.position, m_PathList[m_CurrentHexCount].transform.position) > 0.2f)
			{
				Vector3 direction = m_PathList[m_CurrentHexCount].transform.position - transform.position;

				Vector3 targetRot = transform.position - m_PathList[m_CurrentHexCount].transform.position;
				Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, 10 * Time.deltaTime, 0.0f);

				transform.rotation = Quaternion.LookRotation(newDir);
				transform.Translate(direction.normalized * Time.deltaTime * 15, Space.World);
				yield return new WaitForEndOfFrame();
			}
			else
			{
				if (m_CurrentHexCount + 1 < m_PathList.Count)
				{
					GridManager.instance.ChangeHexMaterial(m_PathList[m_CurrentHexCount], MaterialManager.instance.GetMaterial(MaterialName.BaseMaterial));
					m_CurrentHexCount++;
					yield return new WaitForEndOfFrame();
				}
				yield return new WaitForEndOfFrame();
			}
		}

		transform.LookAt(transformToLookAt);

		m_Animator.SetBool("IsWalking", false);

		m_MoveAmount -= m_PathList.Count;

		m_CurrentHex.m_IsAvailable = true;
		m_CurrentHex.m_CurrentUnit = null;
		m_CurrentHex = m_PathList[m_PathList.Count - 1];
		m_CurrentHex.m_IsAvailable = false;
		m_CurrentHex.m_CurrentUnit = this;

		m_PathList.Clear();

		GridManager.instance.ChangeHexMaterial(m_CurrentHex, MaterialManager.instance.GetMaterial(MaterialName.BaseMaterial));

		m_TargetHealth = targetHex.m_CurrentUnit.m_Health;
		Attack();

		yield return null;
	}

	private void Die()
	{
		UnitManager.instance.FireUnitDiesAction(this);
	}

	public void Attack()
	{
		m_MoveAmount = 0;
		m_Animator.SetTrigger("Attack");
		SelectionManager.instance.SelectUnit(m_CurrentHex, this);
	}

	public void DealDamage()
	{
		m_TargetHealth.TakeDamage(m_AttackDamage);
	}

	public bool CheckIfDead()
	{
		return m_Health.m_IsDead;
	}
}
