using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {

	public delegate void DeathEvent();
	public DeathEvent OnUnitDeath;

	private int m_CurrentHealth;

	public bool m_IsDead = false;

	void Start () {
		
	}
	
	void Update () {
		
	}
	private void OnDestroy()
	{
		Debug.Log("IK BEN DOOD");
	}
	public void TakeDamage(int amount)
	{
		m_CurrentHealth -= amount;

		if (m_CurrentHealth <= 0)
		{
			m_IsDead = true;
			OnUnitDeath?.Invoke();
		}
	}

	public void SetCurrentHealth(int value)
	{
		m_CurrentHealth = value;
	}

	public int GetHealth()
	{
		return m_CurrentHealth;
	}
}
